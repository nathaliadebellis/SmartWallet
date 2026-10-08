using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Web.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string? Email { get; set; }
}
