using SmartWallet.Domain.Common;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Domain.Entities;

public class FinancialTransaction : BaseEntity
{
    public const int DescriptionMaxLength = 150;
    public const int NotesMaxLength = 500;

    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public TransactionType Type { get; private set; }

    public string? Notes { get; private set; }

    public int CategoryId { get; private set; }

    public Category Category { get; private set; } = null!;

    public string ApplicationUserId { get; set; } = string.Empty;

    private FinancialTransaction()
    {
    }

    public FinancialTransaction(
        string description,
        decimal amount,
        DateTime date,
        TransactionType type,
        int categoryId,
        string? notes = null)
    {
        SetDescription(description);
        SetAmount(amount);

        TransactionDate = date;
        Type = type;
        CategoryId = categoryId;
        Notes = notes;

        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string description,
        decimal amount,
        DateTime date,
        TransactionType type,
        int categoryId,
        string? notes)
    {
        SetDescription(description);
        SetAmount(amount);

        TransactionDate = date;
        Type = type;
        CategoryId = categoryId;
        Notes = notes;

        UpdatedAt = DateTime.UtcNow;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição é obrigatória.");

        if (description.Length > DescriptionMaxLength)
            throw new ArgumentException($"A descrição não pode exceder {DescriptionMaxLength} caracteres.");

        Description = description.Trim();
    }

    private void SetAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.");

        Amount = amount;
    }
}