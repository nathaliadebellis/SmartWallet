using SmartWallet.Application.DTOs.Dashboard;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Interfaces;

namespace SmartWallet.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IFinancialTransactionRepository _transactionRepository;

    public DashboardService(IFinancialTransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<DashboardDto> GetDashboardAsync(string userId)
    {
        var totalReceitas = await _transactionRepository.GetTotalByUserAndTypeAsync(userId, TransactionType.Income);
        var totalDespesas = await _transactionRepository.GetTotalByUserAndTypeAsync(userId, TransactionType.Expense);

        var latest = await _transactionRepository.GetLatestByUserAsync(userId, 5);

        var dto = new DashboardDto
        {
            TotalReceitas = totalReceitas,
            TotalDespesas = totalDespesas,
            SaldoAtual = totalReceitas - totalDespesas,
            UltimasTransacoes = latest.Select(t => new RecentTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                Description = t.Description,
                CategoryName = t.Category?.Name ?? string.Empty,
                Amount = t.Amount,
                Type = t.Type.ToString()
            }).ToList()
        };

        return dto;
    }
}
