using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Mappings;
using SmartWallet.Domain.Interfaces;

namespace SmartWallet.Application.Services;

public class FinancialTransactionService : IFinancialTransactionService
{
    private readonly IFinancialTransactionRepository _repository;

    public FinancialTransactionService(
        IFinancialTransactionRepository repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<FinancialTransactionDto>> GetAllAsync()
    {
        var transactions = await _repository.GetAllAsync();

        return transactions.Select(transaction =>
            transaction.ToDto());
    }


    public async Task<FinancialTransactionDto?> GetByIdAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);

        return transaction?.ToDto();
    }


    public async Task CreateAsync(
        CreateFinancialTransactionDto dto)
    {
        var transaction = dto.ToEntity();

        await _repository.AddAsync(transaction);
    }


    public async Task UpdateAsync(
        UpdateFinancialTransactionDto dto)
    {
        var transaction = await _repository.GetByIdAsync(dto.Id);

        if (transaction is null)
            throw new KeyNotFoundException(
                "Transaction not found.");

        transaction.UpdateEntity(dto);

        await _repository.UpdateAsync(transaction);
    }


    public async Task DeleteAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);

        if (transaction is null)
            throw new KeyNotFoundException(
                "Transaction not found.");

        await _repository.DeleteAsync(transaction);
    }
}