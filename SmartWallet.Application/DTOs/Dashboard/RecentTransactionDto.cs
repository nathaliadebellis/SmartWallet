using System;

namespace SmartWallet.Application.DTOs.Dashboard;

public class RecentTransactionDto
{
    public int Id { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;
}
