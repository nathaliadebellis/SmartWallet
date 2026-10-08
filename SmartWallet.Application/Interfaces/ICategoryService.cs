using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(string userId);

    Task<IEnumerable<CategoryDto>> GetByTransactionTypeAsync(
        TransactionType transactionType,
        string userId);

    Task<CategoryDto?> GetByIdAsync(int id, string userId);

    Task CreateAsync(CreateCategoryDto dto, string userId);

    Task CreateDefaultCategoriesAsync(string userId);

    Task UpdateAsync(UpdateCategoryDto dto, string userId);

    Task DeleteAsync(int id, string userId);
}
