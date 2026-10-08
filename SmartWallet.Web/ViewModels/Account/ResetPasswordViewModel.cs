using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Web.ViewModels.Account;

public class ResetPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Token { get; set; }

    [Display(Name = "Nova senha")]
    [Required(ErrorMessage = "O campo Nova senha é obrigatório.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve possuir entre 8 e 100 caracteres.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Confirmar nova senha")]
    [Required(ErrorMessage = "O campo Confirmar nova senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas informadas não coincidem.")]
    public string? ConfirmPassword { get; set; }
}
