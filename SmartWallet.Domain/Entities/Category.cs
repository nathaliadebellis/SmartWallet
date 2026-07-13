using SmartWallet.Domain.Common;

namespace SmartWallet.Domain.Entities;

/// <summary>
/// Representa uma categoria utilizada para classificar transações financeiras.
/// </summary>
public class Category : BaseEntity
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 500;
    public const int IconMaxLength = 50;
    public const int ColorMaxLength = 20;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Icon { get; private set; }

    public string? Color { get; private set; }

    /// <summary>
    /// Transações associadas à categoria.
    /// </summary>
    public ICollection<FinancialTransaction> Transactions { get; private set; } = new List<FinancialTransaction>();

    /// <summary>
    /// Construtor necessário para o Entity Framework Core.
    /// </summary>
    private Category()
    {
    }

    public Category(
        string name,
        string? description = null,
        string? icon = null,
        string? color = null)
    {
        ChangeName(name);
        ChangeDescription(description);
        ChangeIcon(icon);
        ChangeColor(color);
    }

    public void Update(
        string name,
        string? description,
        string? icon,
        string? color)
    {
        ChangeName(name);
        ChangeDescription(description);
        ChangeIcon(icon);
        ChangeColor(color);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        if (name.Length > NameMaxLength)
            throw new ArgumentException(
                $"Category name cannot exceed {NameMaxLength} characters.");

        Name = name.Trim();
    }

    public void ChangeDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) &&
            description.Length > DescriptionMaxLength)
        {
            throw new ArgumentException(
                $"Description cannot exceed {DescriptionMaxLength} characters.");
        }

        Description = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();
    }

    public void ChangeIcon(string? icon)
    {
        if (!string.IsNullOrWhiteSpace(icon) &&
            icon.Length > IconMaxLength)
        {
            throw new ArgumentException(
                $"Icon cannot exceed {IconMaxLength} characters.");
        }

        Icon = string.IsNullOrWhiteSpace(icon)
            ? null
            : icon.Trim();
    }

    public void ChangeColor(string? color)
    {
        if (!string.IsNullOrWhiteSpace(color) &&
            color.Length > ColorMaxLength)
        {
            throw new ArgumentException(
                $"Color cannot exceed {ColorMaxLength} characters.");
        }

        Color = string.IsNullOrWhiteSpace(color)
            ? null
            : color.Trim();
    }
}