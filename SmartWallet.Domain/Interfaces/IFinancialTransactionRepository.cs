using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Filters;
using SmartWallet.Domain.Reports;

namespace SmartWallet.Domain.Interfaces;

public interface IFinancialTransactionRepository
{
    Task<FinancialTransaction?> GetByIdAsync(int id);

    Task<IEnumerable<FinancialTransaction>> GetByUserAsync(string userId);

    Task<PagedResult<FinancialTransaction>> SearchByUserAsync(string userId, TransactionFilter filter);

    Task<decimal> GetTotalByUserAndTypeAsync(string userId, Domain.Enums.TransactionType type);

    Task<IEnumerable<FinancialTransaction>> GetLatestByUserAsync(string userId, int count);

    /// <summary>Totais de despesas por categoria no período [from, to).</summary>
    Task<IReadOnlyList<CategoryTotal>> GetExpenseTotalsByCategoryAsync(string userId, DateTime from, DateTime to);

    /// <summary>Totais por mês e tipo de transação no período [from, to).</summary>
    Task<IReadOnlyList<MonthlyTotal>> GetMonthlyTotalsAsync(string userId, DateTime from, DateTime to);

    Task AddAsync(FinancialTransaction transaction);

    Task UpdateAsync(FinancialTransaction transaction);

    Task DeleteAsync(FinancialTransaction transaction);
}
