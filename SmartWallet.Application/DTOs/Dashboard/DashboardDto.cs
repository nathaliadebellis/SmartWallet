using System.Collections.Generic;

namespace SmartWallet.Application.DTOs.Dashboard;

public class DashboardDto
{
    public decimal TotalIncome { get; set; }

    public decimal TotalExpenses { get; set; }

    public decimal CurrentBalance { get; set; }

    public List<RecentTransactionDto> RecentTransactions { get; set; } = new();

    public List<CategoryExpenseDto> ExpensesByCategory { get; set; } = new();

    public List<MonthlySummaryDto> MonthlySummary { get; set; } = new();
}
