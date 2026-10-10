using Microsoft.Extensions.DependencyInjection;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Services;

namespace SmartWallet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IFinancialTransactionService, FinancialTransactionService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
