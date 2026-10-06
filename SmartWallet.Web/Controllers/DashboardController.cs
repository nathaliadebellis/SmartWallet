using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.Web.ViewModels.Dashboard;
using SmartWallet.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

using SmartWallet.Application.DTOs.Dashboard;

namespace SmartWallet.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        _logger.LogInformation("Carregando Dashboard para usuário {UserId}", userId);

        DashboardDto dto;
        try
        {
            dto = await _dashboardService.GetDashboardAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar dados do Dashboard para usuário {UserId}", userId);
            throw;
        }

        var model = new DashboardViewModel
        {
            TotalReceitas = dto.TotalReceitas,
            TotalDespesas = dto.TotalDespesas,
            SaldoAtual = dto.SaldoAtual,
            NomeUsuario = User?.Identity?.Name ?? string.Empty,
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

