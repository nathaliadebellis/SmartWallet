using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartWallet.Web.ViewModels.Transactions;

public class TransactionListViewModel
{
    public TransactionFilterViewModel Filter { get; set; } = new();

    public IReadOnlyList<TransactionListItemViewModel> Items { get; set; }
        = Array.Empty<TransactionListItemViewModel>();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }
        = Enumerable.Empty<SelectListItem>();

    public IEnumerable<SelectListItem> TransactionTypes { get; set; }
        = Enumerable.Empty<SelectListItem>();

    public bool HasActiveFilters =>
        !string.IsNullOrWhiteSpace(Filter.Search) ||
        Filter.Type.HasValue ||
        Filter.CategoryId.HasValue ||
        Filter.From.HasValue ||
        Filter.To.HasValue;
}
