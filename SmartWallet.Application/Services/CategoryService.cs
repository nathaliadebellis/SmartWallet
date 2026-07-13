using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Interfaces;

namespace SmartWallet.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        return category is null
            ? null
            : MapToDto(category);
    }

    public async Task CreateAsync(CreateCategoryDto dto)
    {
        if (await _categoryRepository.ExistsByNameAsync(dto.Name))
        {
            throw new InvalidOperationException(
                "Já existe uma categoria com esse nome.");
        }

        var category = new Category(
            dto.Name,
            dto.Description,
            dto.Icon,
            dto.Color);

        await _categoryRepository.AddAsync(category);
    }

    public async Task UpdateAsync(UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id);

        if (category is null)
            throw new KeyNotFoundException(
                "Category not found.");

        if (await _categoryRepository.ExistsByNameAsync(dto.Name, dto.Id))
        {
            throw new InvalidOperationException(
                "Já existe uma categoria com esse nome.");
        }

        category.Update(
            dto.Name,
            dto.Description,
            dto.Icon,
            dto.Color);

        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
            throw new KeyNotFoundException(
                "Category not found.");

        await _categoryRepository.DeleteAsync(category);
    }


    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            Color = category.Color
        };
    }
}