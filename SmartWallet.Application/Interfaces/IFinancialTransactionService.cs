using SmartWallet.Application.DTOs;
using SmartWallet.Application.DTOs.FinancialTransactions;

namespace SmartWallet.Application.Interfaces;

public interface IFinancialTransactionService
{
    Task<IEnumerable<FinancialTransactionDto>> GetAllAsync(string userId);

    Task<PagedResultDto<FinancialTransactionDto>> SearchAsync(
        TransactionFilterDto filter,
        string userId);

    Task<FinancialTransactionDto?> GetByIdAsync(int id, string userId);

    Task CreateAsync(CreateFinancialTransactionDto dto, string userId);

    Task UpdateAsync(UpdateFinancialTransactionDto dto, string userId);

    Task DeleteAsync(int id, string userId);
}
