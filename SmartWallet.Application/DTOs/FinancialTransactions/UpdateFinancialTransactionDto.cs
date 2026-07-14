using SmartWallet.Domain.Enums;

namespace SmartWallet.Application.DTOs.FinancialTransactions;

public class UpdateFinancialTransactionDto
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public TransactionType Type { get; set; }

    public int CategoryId { get; set; }

    public string? Notes { get; set; }
}