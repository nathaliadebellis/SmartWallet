using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.Infrastructure.Identity;
using SmartWallet.Web.ViewModels.Account;

namespace SmartWallet.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        try
        {
            _logger.LogInformation("Tentando criar usuário {Email}", model.Email);

            var result = await _userManager.CreateAsync(user, model.Password);

            _logger.LogInformation("Criação de usuário concluída: {Succeeded}", result.Succeeded);

            foreach (var error in result.Errors)
            {
                _logger.LogWarning("Create user error {Code}: {Description}", error.Code, error.Description);
            }

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("Usuário {Email} autenticado após registro.", model.Email);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception ex)
        {
            Console.WriteLine("===== EXCEPTION =====");
            Console.WriteLine(ex.ToString());

            if (ex.InnerException is not null)
            {
                Console.WriteLine("===== INNER EXCEPTION =====");
                Console.WriteLine(ex.InnerException.ToString());
            }

            ModelState.AddModelError(
                string.Empty,
                $"Erro ao criar usuário: {ex.InnerException?.Message ?? ex.Message}");

            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _logger.LogInformation("Tentativa de login para {Email}", model.Email);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        _logger.LogInformation("Resultado do PasswordSignInAsync: Succeeded={Succeeded}, IsLockedOut={IsLockedOut}, RequiresTwoFactor={RequiresTwoFactor}", result.Succeeded, result.IsLockedOut, result.RequiresTwoFactor);

        if (result.Succeeded)
        {
            _logger.LogInformation("Autenticação realizada com sucesso para {Email}", model.Email);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                _logger.LogInformation("Usuário {Email} possui {Count} claims", model.Email, claims.Count);
            }

            try
            {
                var setCookie = HttpContext.Response.Headers["Set-Cookie"].ToString();
                _logger.LogInformation("Set-Cookie header after sign-in: {SetCookie}", setCookie);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Não foi possível ler Set-Cookie header após sign-in.");
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                _logger.LogInformation("Redirecionando para ReturnUrl: {ReturnUrl}", model.ReturnUrl);
                return Redirect(model.ReturnUrl);
            }

            _logger.LogInformation("Redirecionando para Dashboard");
            return RedirectToAction("Index", "Dashboard");
        }

        _logger.LogWarning("Falha na autenticação para {Email}", model.Email);

        ModelState.AddModelError(
            string.Empty,
            "Login inválido. Verifique seu e-mail e senha.");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}