using SmartWallet.Domain.Entities;

namespace SmartWallet.Domain.Interfaces;

public interface IFinancialTransactionRepository
{
    Task<IEnumerable<FinancialTransaction>> GetAllAsync();

    Task<FinancialTransaction?> GetByIdAsync(int id);

    Task AddAsync(FinancialTransaction transaction);

    Task UpdateAsync(FinancialTransaction transaction);

    Task DeleteAsync(FinancialTransaction transaction);
}