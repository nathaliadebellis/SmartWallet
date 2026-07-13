using SmartWallet.Application.DTOs;
using SmartWallet.Application.DTOs.Categories;

namespace SmartWallet.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(int id);

    Task CreateAsync(CreateCategoryDto dto);

    Task UpdateAsync(UpdateCategoryDto dto);

    Task DeleteAsync(int id);
}