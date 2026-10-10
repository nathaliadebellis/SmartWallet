using System;

namespace SmartWallet.Web.ViewModels.Dashboard;

public class DashboardViewModel
{
    public decimal TotalIncome { get; set; }

    public decimal TotalExpenses { get; set; }

    public decimal CurrentBalance { get; set; }

    public string? UserName { get; set; }

    public List<RecentTransactionViewModel> RecentTransactions { get; set; } = new();

    public string CurrentMonthLabel { get; set; } = string.Empty;

    public List<CategoryExpenseViewModel> ExpensesByCategory { get; set; } = new();

    public List<MonthlySummaryViewModel> MonthlySummary { get; set; } = new();

    public bool HasMonthlySummary => MonthlySummary.Any(m => m.Income > 0 || m.Expenses > 0);
}
