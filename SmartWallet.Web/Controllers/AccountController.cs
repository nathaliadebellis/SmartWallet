using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SmartWallet.Application.Interfaces;
using SmartWallet.Infrastructure.Identity;
using SmartWallet.Web.ViewModels.Account;

namespace SmartWallet.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICategoryService _categoryService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ICategoryService categoryService,
        IEmailSender emailSender,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _categoryService = categoryService;
        _emailSender = emailSender;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction("Index", "Dashboard");

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
            Email = model.Email,
            FullName = model.Name?.Trim()
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _categoryService.CreateDefaultCategoriesAsync(user.Id);

        await _signInManager.SignInAsync(user, isPersistent: false);

        _logger.LogInformation("Novo usuário {UserId} cadastrado.", user.Id);

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction("Index", "Dashboard");

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

        var result = await _signInManager.PasswordSignInAsync(
            model.Email!,
            model.Password!,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return LocalRedirect(model.ReturnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("Conta bloqueada por excesso de tentativas de login.");

            ModelState.AddModelError(
                string.Empty,
                "Muitas tentativas de login. Tente novamente em alguns minutos.");

            return View(model);
        }

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

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email!);

        // A mesma resposta é dada exista ou não a conta, para não revelar
        // quais e-mails estão cadastrados.
        if (user is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { email = user.Email, token = encodedToken },
                Request.Scheme)!;

            await _emailSender.SendAsync(
                user.Email!,
                "Redefinição de senha - SmartWallet",
                $"<p>Para redefinir sua senha, <a href=\"{HtmlEncoder.Default.Encode(link)}\">clique aqui</a>.</p>" +
                "<p>Se você não solicitou, ignore este e-mail.</p>");
        }

        return RedirectToAction(nameof(ForgotPasswordConfirmation));
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string? email = null, string? token = null)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return BadRequest("Link de redefinição inválido.");

        return View(new ResetPasswordViewModel
        {
            Email = email,
            Token = token
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email!);

        if (user is null)
            return RedirectToAction(nameof(ResetPasswordConfirmation));

        string token;
        try
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token!));
        }
        catch (FormatException)
        {
            ModelState.AddModelError(string.Empty, "Link de redefinição inválido ou expirado.");
            return View(model);
        }

        var result = await _userManager.ResetPasswordAsync(user, token, model.Password!);

        if (result.Succeeded)
            return RedirectToAction(nameof(ResetPasswordConfirmation));

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
}
