using System;

namespace SmartWallet.Web.ViewModels.Dashboard;

public class DashboardViewModel
{
    public decimal TotalReceitas { get; set; }

    public decimal TotalDespesas { get; set; }

    public decimal SaldoAtual { get; set; }

    public string? NomeUsuario { get; set; }
    public List<RecentTransactionViewModel> UltimasTransacoes { get; set; } = new();

}
