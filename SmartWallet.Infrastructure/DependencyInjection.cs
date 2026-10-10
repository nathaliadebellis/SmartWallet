using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartWallet.Application.Interfaces;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Infrastructure.Data;
using SmartWallet.Infrastructure.Repositories;
using SmartWallet.Infrastructure.Services;

namespace SmartWallet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("SmartWallet.Infrastructure");
                    sqlOptions.EnableRetryOnFailure();
                }));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        if (!string.IsNullOrWhiteSpace(configuration["Email:Smtp:Host"]))
        {
            services.AddScoped<IEmailSender, SmtpEmailSender>();
        }
        else
        {
            services.AddScoped<IEmailSender>(sp => new LoggingEmailSender(
                sp.GetRequiredService<ILogger<LoggingEmailSender>>(),
                isDevelopment));
        }

        return services;
    }
}
