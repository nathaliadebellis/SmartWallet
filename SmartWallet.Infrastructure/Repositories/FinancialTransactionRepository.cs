using Microsoft.EntityFrameworkCore;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Filters;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Reports;
using SmartWallet.Infrastructure.Data;

namespace SmartWallet.Infrastructure.Repositories;

public class FinancialTransactionRepository : IFinancialTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public FinancialTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialTransaction?> GetByIdAsync(int id)
    {
        return await _context.FinancialTransactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<FinancialTransaction>> GetByUserAsync(string userId)
    {
        return await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId)
            .Include(t => t.Category)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .ThenByDescending(t => t.Id)
            .ToListAsync();
    }

    public async Task<PagedResult<FinancialTransaction>> SearchByUserAsync(
        string userId,
        TransactionFilter filter)
    {
        var query = _context.FinancialTransactions
            .AsNoTracking()
            .Where(t => t.ApplicationUserId == userId);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();

            query = query.Where(t =>
                t.Description.Contains(term) ||
                t.Category.Name.Contains(term));
        }

        if (filter.Type.HasValue)
            query = query.Where(t => t.Type == filter.Type.Value);

        if (filter.CategoryId.HasValue)
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);

        if (filter.From.HasValue)
            query = query.Where(t => t.TransactionDate >= filter.From.Value.Date);

        if (filter.To.HasValue)
            query = query.Where(t => t.TransactionDate < filter.To.Value.Date.AddDays(1));

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(t => t.Category)
            .OrderByDescending(t => t.TransactionDate)
            .ThenByDescending(t => t.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<FinancialTransaction>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<decimal> GetTotalByUserAndTypeAsync(string userId, Domain.Enums.TransactionType type)
    {
        return await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId && t.Type == type)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
    }

    public async Task<IEnumerable<FinancialTransaction>> GetLatestByUserAsync(string userId, int count)
    {
        return await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId)
            .Include(t => t.Category)
            .AsNoTracking()
            .OrderByDescending(t => t.TransactionDate)
            .ThenByDescending(t => t.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<CategoryTotal>> GetExpenseTotalsByCategoryAsync(
        string userId,
        DateTime from,
        DateTime to)
    {
        var totals = await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId
                && t.Type == Domain.Enums.TransactionType.Expense
                && t.TransactionDate >= from
                && t.TransactionDate < to)
            .GroupBy(t => new { t.CategoryId, t.Category.Name })
            .Select(g => new
            {
                g.Key.Name,
                Total = g.Sum(t => t.Amount)
            })
            .OrderByDescending(g => g.Total)
            .ToListAsync();

        return totals
            .Select(t => new CategoryTotal(t.Name, t.Total))
            .ToList();
    }

    public async Task<IReadOnlyList<MonthlyTotal>> GetMonthlyTotalsAsync(
        string userId,
        DateTime from,
        DateTime to)
    {
        var totals = await _context.FinancialTransactions
            .Where(t => t.ApplicationUserId == userId
                && t.TransactionDate >= from
                && t.TransactionDate < to)
            .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month, t.Type })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                g.Key.Type,
                Total = g.Sum(t => t.Amount)
            })
            .ToListAsync();

        return totals
            .Select(t => new MonthlyTotal(t.Year, t.Month, t.Type, t.Total))
            .ToList();
    }

    public async Task AddAsync(FinancialTransaction transaction)
    {
        await _context.FinancialTransactions.AddAsync(transaction);
    }

    public Task UpdateAsync(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Update(transaction);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Remove(transaction);

        return Task.CompletedTask;
    }
}