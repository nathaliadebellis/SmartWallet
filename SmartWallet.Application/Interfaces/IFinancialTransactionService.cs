using SmartWallet.Application.DTOs.FinancialTransactions;

namespace SmartWallet.Application.Interfaces;

public interface IFinancialTransactionService
{
    Task<IEnumerable<FinancialTransactionDto>> GetAllAsync();

    Task<FinancialTransactionDto?> GetByIdAsync(int id, string userId);

    Task CreateAsync(CreateFinancialTransactionDto dto, string userId);

    Task UpdateAsync(UpdateFinancialTransactionDto dto);

    Task DeleteAsync(int id);
}