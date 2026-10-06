using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Web.ViewModels.Account;

public class LoginViewModel
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string? Email { get; set; }

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Lembrar-me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
