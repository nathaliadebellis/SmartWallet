using SmartWallet.Domain.Common;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Domain.Entities;

/// <summary>
/// Representa uma movimentação financeira.
/// </summary>
public class FinancialTransaction : BaseEntity
{
    public const int DescriptionMaxLength = 200;
    public const int NotesMaxLength = 1000;

    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public TransactionType Type { get; private set; }

    public string? Notes { get; private set; }

    public int CategoryId { get; private set; }

    public Category Category { get; private set; } = null!;


    private FinancialTransaction()
    {
        // Necessário para o Entity Framework Core.
    }


    public FinancialTransaction(
        string description,
        decimal amount,
        DateTime transactionDate,
        TransactionType type,
        int categoryId,
        string? notes = null)
    {
        ChangeDescription(description);
        ChangeAmount(amount);
        ChangeTransactionDate(transactionDate);
        ChangeType(type);
        ChangeCategory(categoryId);
        ChangeNotes(notes);
    }


    public void Update(
        string description,
        decimal amount,
        DateTime transactionDate,
        TransactionType type,
        int categoryId,
        string? notes)
    {
        ChangeDescription(description);
        ChangeAmount(amount);
        ChangeTransactionDate(transactionDate);
        ChangeType(type);
        ChangeCategory(categoryId);
        ChangeNotes(notes);
    }


    public void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.");

        if (description.Length > DescriptionMaxLength)
            throw new ArgumentException(
                $"Description cannot exceed {DescriptionMaxLength} characters.");

        Description = description.Trim();
    }


    public void ChangeAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.");

        Amount = amount;
    }


    public void ChangeTransactionDate(DateTime date)
    {
        TransactionDate = date;
    }


    public void ChangeType(TransactionType type)
    {
        Type = type;
    }


    public void ChangeCategory(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentException(
                "Category is required.");

        CategoryId = categoryId;
    }


    public void ChangeNotes(string? notes)
    {
        if (!string.IsNullOrWhiteSpace(notes) &&
            notes.Length > NotesMaxLength)
        {
            throw new ArgumentException(
                $"Notes cannot exceed {NotesMaxLength} characters.");
        }

        Notes = string.IsNullOrWhiteSpace(notes)
            ? null
            : notes.Trim();
    }
}