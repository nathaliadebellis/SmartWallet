using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Web.ViewModels.Transactions;

public class TransactionFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe a descrição.")]
    [StringLength(150, ErrorMessage = "A descrição deve possuir no máximo 150 caracteres.")]
    [Display(Name = "Descrição")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o valor.")]
    [Range(0.01, 999999999.99,
    ErrorMessage = "Informe um valor maior que zero.")]
    [Display(Name = "Valor")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Informe a data da transação.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data")]
    public DateTime TransactionDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Selecione o tipo da transação.")]
    [Display(Name = "Tipo")]
    public TransactionType Type { get; set; }

    [Required(ErrorMessage = "Selecione uma categoria.")]
    [Display(Name = "Categoria")]
    public int CategoryId { get; set; }

    [StringLength(500, ErrorMessage = "As observações devem possuir no máximo 500 caracteres.")]
    [Display(Name = "Observações")]
    public string? Notes { get; set; }

    // Lista de categorias para o DropDown
    public IEnumerable<SelectListItem> Categories { get; set; }
        = Enumerable.Empty<SelectListItem>();

    // Lista dos tipos (Receita / Despesa)
    public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        = Enumerable.Empty<SelectListItem>();
}