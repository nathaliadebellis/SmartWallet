using SmartWallet.Application.DTOs.Dashboard;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Reports;

namespace SmartWallet.Application.Services;

public class DashboardService : IDashboardService
{
    public const int MonthsInSummary = 6;
    public const int MaxCategoriesInChart = 6;
    public const string OtherCategoriesName = "Outras";

    private const int RecentTransactionsCount = 5;

    private readonly IFinancialTransactionRepository _transactionRepository;

    public DashboardService(IFinancialTransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<DashboardDto> GetDashboardAsync(string userId)
    {
        var totalIncome = await _transactionRepository.GetTotalByUserAndTypeAsync(userId, TransactionType.Income);
        var totalExpenses = await _transactionRepository.GetTotalByUserAndTypeAsync(userId, TransactionType.Expense);

        var latest = await _transactionRepository.GetLatestByUserAsync(userId, RecentTransactionsCount);

        var currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var nextMonth = currentMonth.AddMonths(1);
        var firstMonth = currentMonth.AddMonths(-(MonthsInSummary - 1));

        var categoryTotals = await _transactionRepository
            .GetExpenseTotalsByCategoryAsync(userId, currentMonth, nextMonth);

        var monthlyTotals = await _transactionRepository
            .GetMonthlyTotalsAsync(userId, firstMonth, nextMonth);

        var dto = new DashboardDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            CurrentBalance = totalIncome - totalExpenses,
            RecentTransactions = latest.Select(t => new RecentTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                CategoryName = t.Category?.Name ?? string.Empty,
                Amount = t.Amount,
                Type = t.Type.ToString()
            }).ToList(),
            ExpensesByCategory = BuildExpensesByCategory(categoryTotals),
            MonthlySummary = BuildMonthlySummary(monthlyTotals, firstMonth)
        };

        return dto;
    }

    private static List<CategoryExpenseDto> BuildExpensesByCategory(IReadOnlyList<CategoryTotal> totals)
    {
        var ordered = totals
            .OrderByDescending(c => c.Total)
            .Select(c => new CategoryExpenseDto
            {
                CategoryName = c.CategoryName,
                Total = c.Total
            })
            .ToList();

        if (ordered.Count <= MaxCategoriesInChart)
            return ordered;

        // Mantém o gráfico legível: as menores categorias viram uma única barra.
        var top = ordered.Take(MaxCategoriesInChart - 1).ToList();

        top.Add(new CategoryExpenseDto
        {
            CategoryName = OtherCategoriesName,
            Total = ordered.Skip(MaxCategoriesInChart - 1).Sum(c => c.Total)
        });

        return top;
    }

    private static List<MonthlySummaryDto> BuildMonthlySummary(
        IReadOnlyList<MonthlyTotal> totals,
        DateTime firstMonth)
    {
        // Meses sem transações também aparecem, com valores zerados.
        return Enumerable.Range(0, MonthsInSummary)
            .Select(offset => firstMonth.AddMonths(offset))
            .Select(month => new MonthlySummaryDto
            {
                Year = month.Year,
                Month = month.Month,
                Income = SumMonth(totals, month, TransactionType.Income),
                Expenses = SumMonth(totals, month, TransactionType.Expense)
            })
            .ToList();
    }

    private static decimal SumMonth(IReadOnlyList<MonthlyTotal> totals, DateTime month, TransactionType type)
    {
        return totals
            .Where(t => t.Year == month.Year && t.Month == month.Month && t.Type == type)
            .Sum(t => t.Total);
    }
}
