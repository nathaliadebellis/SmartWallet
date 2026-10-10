using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using SmartWallet.Infrastructure.Data;
using SmartWallet.Infrastructure.Identity;
using SmartWallet.Web.Binders;
using SmartWallet.Web.Localization;

namespace SmartWallet.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebMvc(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
            })
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

        return services;
    }

    public static IServiceCollection AddIdentityAndCookies(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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
            .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(2));

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
        });

        return services;
    }
}
