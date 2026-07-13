using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Web.ViewModels.Categories;

public class CategoryFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome da categoria.")]
    [StringLength(100, ErrorMessage = "O nome deve possuir no máximo 100 caracteres.")]
    [Display(Name = "Nome")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve possuir no máximo 500 caracteres.")]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Display(Name = "Ícone")]
    public string? Icon { get; set; }

    [Display(Name = "Cor")]
    public string? Color { get; set; }
}