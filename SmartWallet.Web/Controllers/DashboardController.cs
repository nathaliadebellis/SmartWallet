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
            TotalReceitas = dto.TotalReceitas,
            TotalDespesas = dto.TotalDespesas,
            SaldoAtual = dto.SaldoAtual,
            NomeUsuario = user?.FullName ?? user?.Email ?? User.Identity?.Name,
            UltimasTransacoes = dto.UltimasTransacoes.Select(t => new RecentTransactionViewModel
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                CategoryName = t.CategoryName,
                Amount = t.Amount,
                Type = t.Type
            }).ToList()
        };

        return View(model);
    }
}
