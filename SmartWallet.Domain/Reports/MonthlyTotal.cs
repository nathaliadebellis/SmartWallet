using SmartWallet.Domain.Enums;

namespace SmartWallet.Domain.Reports;

public record MonthlyTotal(int Year, int Month, TransactionType Type, decimal Total);
