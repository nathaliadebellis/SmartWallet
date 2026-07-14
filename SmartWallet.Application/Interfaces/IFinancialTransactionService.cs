using SmartWallet.Application.DTOs.FinancialTransactions;

namespace SmartWallet.Application.Interfaces;

public interface IFinancialTransactionService
{
    Task<IEnumerable<FinancialTransactionDto>> GetAllAsync();

    Task<FinancialTransactionDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateFinancialTransactionDto dto);

    Task UpdateAsync(UpdateFinancialTransactionDto dto);

    Task DeleteAsync(int id);
}