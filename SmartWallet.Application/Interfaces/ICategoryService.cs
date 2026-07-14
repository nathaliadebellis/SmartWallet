using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<IEnumerable<CategoryDto>> GetByTransactionTypeAsync(
        TransactionType transactionType);

    Task<CategoryDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateCategoryDto dto);

    Task UpdateAsync(UpdateCategoryDto dto);

    Task DeleteAsync(int id);
}