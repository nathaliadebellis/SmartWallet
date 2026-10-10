using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.Application.Interfaces;
using SmartWallet.Infrastructure.Identity;
using SmartWallet.Web.Extensions;
using SmartWallet.Web.ViewModels.Dashboard;

namespace SmartWallet.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(
        IDashboardService dashboardService,
        UserManager<ApplicationUser> userManager)
    {
        _dashboardService = dashboardService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();

        var dto = await _dashboardService.GetDashboardAsync(userId);
        var user = await _userManager.GetUserAsync(User);

        var model = new DashboardViewModel
        {
            TotalIncome = dto.TotalIncome,
            TotalExpenses = dto.TotalExpenses,
            CurrentBalance = dto.CurrentBalance,
            UserName = user?.FullName ?? user?.Email ?? User.Identity?.Name,
            RecentTransactions = dto.RecentTransactions.Select(t => new RecentTransactionViewModel
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                CategoryName = t.CategoryName,
                Amount = t.Amount,
                Type = t.Type
            }).ToList(),
            CurrentMonthLabel = DateTime.Today.ToString("MMMM 'de' yyyy", CultureInfo.CurrentCulture),
            ExpensesByCategory = dto.ExpensesByCategory.Select(c => new CategoryExpenseViewModel
            {
                CategoryName = c.CategoryName,
                Total = c.Total
            }).ToList(),
            MonthlySummary = dto.MonthlySummary.Select(m => new MonthlySummaryViewModel
            {
                Label = FormatMonthLabel(m.Year, m.Month),
                Income = m.Income,
                Expenses = m.Expenses
            }).ToList()
        };

        return View(model);
    }

    private static string FormatMonthLabel(int year, int month)
    {
        var date = new DateTime(year, month, 1);

        // "out./26" -> "out/26"
        var monthName = date.ToString("MMM", CultureInfo.CurrentCulture).TrimEnd('.');

        return $"{monthName}/{date:yy}";
    }
}
