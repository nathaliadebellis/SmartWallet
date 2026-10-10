using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Services;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Infrastructure.Data;
using SmartWallet.Infrastructure.Identity;
using SmartWallet.Infrastructure.Repositories;
using SmartWallet.Infrastructure.Services;
using SmartWallet.Web.Binders;
using SmartWallet.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("SmartWallet.Infrastructure");
            sqlOptions.EnableRetryOnFailure();
        }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddErrorDescriber<SmartWallet.Web.Localization.PortugueseIdentityErrorDescriber>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IFinancialTransactionRepository, FinancialTransactionRepository>();
builder.Services.AddScoped<IFinancialTransactionService, FinancialTransactionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

if (!string.IsNullOrWhiteSpace(builder.Configuration["Email:Smtp:Host"]))
{
    builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
}
else
{
    builder.Services.AddScoped<IEmailSender>(sp => new LoggingEmailSender(
        sp.GetRequiredService<ILogger<LoggingEmailSender>>(),
        builder.Environment.IsDevelopment()));
}

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("pt-BR") };
var requestLocalizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures.ToList(),
    SupportedUICultures = supportedCultures.ToList()
};

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    await next();
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        // Usuário de desenvolvimento: só é criado em Development e se a senha
        // estiver configurada (dotnet user-secrets set "Seed:AdminPassword" "...").
        var adminPassword = builder.Configuration["Seed:AdminPassword"];

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

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static async Task SeedAdminAsync(IServiceProvider services, string adminPassword)
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
