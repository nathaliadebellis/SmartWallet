using SmartWallet.Domain.Entities;

namespace SmartWallet.Domain.Interfaces;

public interface IFinancialTransactionRepository
{
    Task<IEnumerable<FinancialTransaction>> GetAllAsync();

    Task<FinancialTransaction?> GetByIdAsync(int id);

    Task<IEnumerable<FinancialTransaction>> GetByUserAsync(string userId);

    Task<decimal> GetTotalByUserAndTypeAsync(string userId, Domain.Enums.TransactionType type);

    Task<IEnumerable<FinancialTransaction>> GetLatestByUserAsync(string userId, int count);

    Task AddAsync(FinancialTransaction transaction);

    Task UpdateAsync(FinancialTransaction transaction);

    Task DeleteAsync(FinancialTransaction transaction);
}