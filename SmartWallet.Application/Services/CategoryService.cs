using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Exceptions;

namespace SmartWallet.Application.Services;

public class CategoryService : ICategoryService
{
    private static readonly (string Name, TransactionType Type)[] DefaultCategories =
    {
        ("Salário", TransactionType.Income),
        ("Outras receitas", TransactionType.Income),
        ("Alimentação", TransactionType.Expense),
        ("Moradia", TransactionType.Expense),
        ("Transporte", TransactionType.Expense),
        ("Saúde", TransactionType.Expense),
        ("Lazer", TransactionType.Expense),
        ("Outras despesas", TransactionType.Expense)
    };

    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(string userId)
    {
        var categories = await _categoryRepository.GetAllAsync(userId);

        return categories.Select(MapToDto);
    }

    public async Task<IEnumerable<CategoryDto>> GetByTransactionTypeAsync(
        TransactionType transactionType,
        string userId)
    {
        var categories = await _categoryRepository
            .GetByTransactionTypeAsync(transactionType, userId);

        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, string userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id, userId);

        return category is null
            ? null
            : MapToDto(category);
    }

    public async Task CreateAsync(CreateCategoryDto dto, string userId)
    {
        if (await _categoryRepository.ExistsByNameAsync(dto.Name, userId))
        {
            throw new DomainException(
                "Já existe uma categoria com esse nome.");
        }

        var category = new Category(
            dto.Name,
            dto.TransactionType,
            dto.Description,
            dto.Icon,
            dto.Color)
        {
            ApplicationUserId = userId
        };

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateDefaultCategoriesAsync(string userId)
    {
        var categories = DefaultCategories.Select(c =>
            new Category(c.Name, c.Type) { ApplicationUserId = userId });

        await _categoryRepository.AddRangeAsync(categories);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateCategoryDto dto, string userId)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id, userId);

        if (category is null)
            throw new NotFoundException(
                "Categoria não encontrada.");

        if (await _categoryRepository.ExistsByNameAsync(dto.Name, userId, dto.Id))
        {
            throw new DomainException(
                "Já existe uma categoria com esse nome.");
        }

        category.Update(
            dto.Name,
            dto.TransactionType,
            dto.Description,
            dto.Icon,
            dto.Color);

        await _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id, userId);

        if (category is null)
            throw new NotFoundException(
                "Categoria não encontrada.");

        if (await _categoryRepository.HasTransactionsAsync(id, userId))
        {
            throw new DomainException(
                "Não é possível excluir uma categoria que possui transações.");
        }

        await _categoryRepository.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            Color = category.Color,
            TransactionType = category.TransactionType
        };
    }
}
