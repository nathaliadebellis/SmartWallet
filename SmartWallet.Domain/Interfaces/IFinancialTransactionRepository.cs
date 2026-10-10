using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Filters;

namespace SmartWallet.Domain.Interfaces;

public interface IFinancialTransactionRepository
{
    Task<FinancialTransaction?> GetByIdAsync(int id);

    Task<IEnumerable<FinancialTransaction>> GetByUserAsync(string userId);

    Task<PagedResult<FinancialTransaction>> SearchByUserAsync(string userId, TransactionFilter filter);

    Task<decimal> GetTotalByUserAndTypeAsync(string userId, Domain.Enums.TransactionType type);

    Task<IEnumerable<FinancialTransaction>> GetLatestByUserAsync(string userId, int count);

    Task AddAsync(FinancialTransaction transaction);

    Task UpdateAsync(FinancialTransaction transaction);

    Task DeleteAsync(FinancialTransaction transaction);
}
