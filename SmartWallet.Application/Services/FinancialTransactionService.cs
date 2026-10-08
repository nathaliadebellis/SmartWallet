using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Mappings;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Exceptions;

namespace SmartWallet.Application.Services;

public class FinancialTransactionService : IFinancialTransactionService
{
    private readonly IFinancialTransactionRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FinancialTransactionService(
        IFinancialTransactionRepository repository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<FinancialTransactionDto>> GetAllAsync(string userId)
    {
        var transactions = await _repository.GetByUserAsync(userId);

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
        await EnsureCategoryIsValidAsync(dto.CategoryId, dto.Type, userId);

        var transaction = dto.ToEntity();

        transaction.ApplicationUserId = userId;

        await _repository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task UpdateAsync(
        UpdateFinancialTransactionDto dto,
        string userId)
    {
        var transaction = await _repository.GetByIdAsync(dto.Id);

        if (transaction is null || transaction.ApplicationUserId != userId)
            throw new NotFoundException(
                "Transação não encontrada.");

        await EnsureCategoryIsValidAsync(dto.CategoryId, dto.Type, userId);

        transaction.UpdateEntity(dto);

        await _repository.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id, string userId)
    {
        var transaction = await _repository.GetByIdAsync(id);

        if (transaction is null || transaction.ApplicationUserId != userId)
            throw new NotFoundException(
                "Transação não encontrada.");

        await _repository.DeleteAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
    }


    private async Task EnsureCategoryIsValidAsync(
        int categoryId,
        TransactionType type,
        string userId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, userId);

        if (category is null)
            throw new DomainException("Categoria inválida.");

        if (category.TransactionType != type)
            throw new DomainException(
                "A categoria selecionada não corresponde ao tipo da transação.");
    }
}
