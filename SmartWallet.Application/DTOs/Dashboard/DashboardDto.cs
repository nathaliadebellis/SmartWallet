using System.Collections.Generic;

namespace SmartWallet.Application.DTOs.Dashboard;

public class DashboardDto
{
    public decimal TotalReceitas { get; set; }

    public decimal TotalDespesas { get; set; }

    public decimal SaldoAtual { get; set; }

    public List<RecentTransactionDto> UltimasTransacoes { get; set; } = new();
}
