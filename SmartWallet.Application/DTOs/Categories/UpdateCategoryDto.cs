using SmartWallet.Domain.Enums;

namespace SmartWallet.Application.DTOs.Categories;

public class UpdateCategoryDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? Icon { get; init; }

    public string? Color { get; init; }

    public TransactionType TransactionType { get; set; }
}