namespace SmartWallet.Web.ViewModels.Dashboard;

public class MonthlySummaryViewModel
{
    public string Label { get; set; } = string.Empty;

    public decimal Income { get; set; }

    public decimal Expenses { get; set; }
}
