using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Application.Interfaces;
using SmartWallet.Infrastructure.Data;
using SmartWallet.Infrastructure.Identity;

namespace SmartWallet.Web.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateAndSeedAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();

            // Usuário de desenvolvimento: só é criado em Development e se a senha
            // estiver configurada (dotnet user-secrets set "Seed:AdminPassword" "...").
            var adminPassword = app.Configuration["Seed:AdminPassword"];

            if (app.Environment.IsDevelopment() && !string.IsNullOrWhiteSpace(adminPassword))
            {
                await SeedAdminAsync(services, adminPassword);
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogCritical(
                ex,
                "Erro durante aplicação das migrations ou seed do Identity. A aplicação não será iniciada.");

            // Sem banco atualizado a aplicação falharia em toda requisição; melhor não subir.
            throw;
        }
    }

    private static async Task SeedAdminAsync(IServiceProvider services, string adminPassword)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var categoryService = services.GetRequiredService<ICategoryService>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        const string adminEmail = "admin@smartwallet.com";

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
            return;

        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Administrador",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await categoryService.CreateDefaultCategoriesAsync(adminUser.Id);

            logger.LogInformation("Usuário administrador de desenvolvimento criado.");
        }
        else
        {
            logger.LogWarning(
                "Falha ao criar usuário administrador: {Errors}",
                string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }
}
