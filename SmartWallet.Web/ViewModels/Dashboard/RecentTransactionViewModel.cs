using System;

namespace SmartWallet.Web.ViewModels.Dashboard;

public class RecentTransactionViewModel
{
    public int Id { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;
}
