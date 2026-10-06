using SmartWallet.Application.DTOs.Dashboard;

namespace SmartWallet.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync(string userId);
}
