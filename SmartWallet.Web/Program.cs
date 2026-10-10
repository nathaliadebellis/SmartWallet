using System.Globalization;
using SmartWallet.Application;
using SmartWallet.Infrastructure;
using SmartWallet.Web.Extensions;
using SmartWallet.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWebMvc()
    .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
    .AddIdentityAndCookies()
    .AddApplication();

var app = builder.Build();

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

await app.MigrateAndSeedAsync();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
