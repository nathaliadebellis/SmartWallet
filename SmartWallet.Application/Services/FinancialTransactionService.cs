using SmartWallet.Application.DTOs;
using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Mappings;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Filters;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Exceptions;

namespace SmartWallet.Application.Services;

public class FinancialTransactionService : IFinancialTransactionService
{
    public const int MaxPageSize = 100;

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


    public async Task<PagedResultDto<FinancialTransactionDto>> SearchAsync(
        TransactionFilterDto filter,
        string userId)
    {
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);
        var from = filter.From;
        var to = filter.To;

        if (from.HasValue && to.HasValue && from > to)
            (from, to) = (to, from);

        var domainFilter = new TransactionFilter
        {
            Search = filter.Search,
            Type = filter.Type,
            CategoryId = filter.CategoryId,
            From = from,
            To = to,
            Page = Math.Max(filter.Page, 1),
            PageSize = pageSize
        };

        var result = await _repository.SearchByUserAsync(userId, domainFilter);

        // Página fora do intervalo (ex.: após excluir o último item da última página).
        if (result.Items.Count == 0 && result.TotalCount > 0 && result.TotalPages > 0)
        {
            domainFilter = new TransactionFilter
            {
                Search = domainFilter.Search,
                Type = domainFilter.Type,
                CategoryId = domainFilter.CategoryId,
                From = domainFilter.From,
                To = domainFilter.To,
                Page = result.TotalPages,
                PageSize = pageSize
            };

            result = await _repository.SearchByUserAsync(userId, domainFilter);
        }

        return new PagedResultDto<FinancialTransactionDto>
        {
            Items = result.Items.Select(t => t.ToDto()).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
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
