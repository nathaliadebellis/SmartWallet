using SmartWallet.Domain.Enums;

namespace SmartWallet.Web.ViewModels.Transactions;

public class TransactionListItemViewModel
{
    public int Id { get; set; }

    public TransactionType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }
}
