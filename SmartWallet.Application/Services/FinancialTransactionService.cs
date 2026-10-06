using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Mappings;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Exceptions;

namespace SmartWallet.Application.Services;

public class FinancialTransactionService : IFinancialTransactionService
{
    private readonly IFinancialTransactionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public FinancialTransactionService(
        IFinancialTransactionRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<FinancialTransactionDto>> GetAllAsync()
    {
        var transactions = await _repository.GetAllAsync();

        return transactions.Select(transaction =>
            transaction.ToDto());
    }


    public async Task<FinancialTransactionDto?> GetByIdAsync(int id, string userId)
    {
        var transaction = await _repository.GetByIdAsync(id);

        if (transaction is null)
            return null;

        if (transaction.ApplicationUserId != userId)
            return null;

        return transaction.ToDto();
    }


    public async Task CreateAsync(
        CreateFinancialTransactionDto dto,
        string userId)
    {
        var transaction = dto.ToEntity();

        transaction.ApplicationUserId = userId;

        await _repository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task UpdateAsync(
        UpdateFinancialTransactionDto dto)
    {
        var transaction = await _repository.GetByIdAsync(dto.Id);

        if (transaction is null)
            throw new NotFoundException(
                "Transação não encontrada.");

        transaction.UpdateEntity(dto);

        await _repository.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);

        if (transaction is null)
            throw new NotFoundException(
                "Transação não encontrada.");

        await _repository.DeleteAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }
}