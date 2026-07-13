namespace SmartWallet.Application.DTOs.Categories;

public class CategoryDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? Icon { get; init; }

    public string? Color { get; init; }
}