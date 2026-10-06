using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Web.ViewModels.Account;

public class RegisterViewModel
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string? Name { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string? Email { get; set; }

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Confirmar Senha")]
    [Required(ErrorMessage = "O campo Confirmar Senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas informadas não coincidem.")]
    public string? ConfirmPassword { get; set; }
}
