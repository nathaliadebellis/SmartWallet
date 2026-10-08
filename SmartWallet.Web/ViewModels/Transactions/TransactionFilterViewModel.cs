using System.ComponentModel.DataAnnotations;
using SmartWallet.Domain.Enums;

namespace SmartWallet.Web.ViewModels.Transactions;

public class TransactionFilterViewModel
{
    public static readonly int[] PageSizeOptions = { 5, 10, 20, 50 };

    [StringLength(150)]
    [Display(Name = "Pesquisar")]
    public string? Search { get; set; }

    [Display(Name = "Tipo")]
    public TransactionType? Type { get; set; }

    [Display(Name = "Categoria")]
    public int? CategoryId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "De")]
    public DateTime? From { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Até")]
    public DateTime? To { get; set; }

    public int Page { get; set; } = 1;

    [Display(Name = "Itens por página")]
    public int PageSize { get; set; } = 10;
}
