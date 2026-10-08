using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Web.ViewModels.Account;

public class RegisterViewModel
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O campo Nome deve possuir no máximo 100 caracteres.")]
    public string? Name { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(256, ErrorMessage = "O campo E-mail deve possuir no máximo 256 caracteres.")]
    public string? Email { get; set; }

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve possuir entre 8 e 100 caracteres.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Confirmar Senha")]
    [Required(ErrorMessage = "O campo Confirmar Senha é obrigatório.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas informadas não coincidem.")]
    public string? ConfirmPassword { get; set; }
}
