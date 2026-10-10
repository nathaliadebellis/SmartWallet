using SmartWallet.Domain.Enums;

namespace SmartWallet.Domain.Filters;

public class TransactionFilter
{
    public string? Search { get; init; }

    public TransactionType? Type { get; init; }

    public int? CategoryId { get; init; }

    public DateTime? From { get; init; }

    public DateTime? To { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
