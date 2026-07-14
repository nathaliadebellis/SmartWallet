using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Domain.Entities;

namespace SmartWallet.Application.Mappings;

public static class FinancialTransactionMappings
{
    public static FinancialTransactionDto ToDto(
        this FinancialTransaction transaction)
    {
        return new FinancialTransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            CategoryName = transaction.Category?.Name ?? string.Empty,
            Notes = transaction.Notes,
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }

    public static FinancialTransaction ToEntity(
        this CreateFinancialTransactionDto dto)
    {
        return new FinancialTransaction(
            dto.Description,
            dto.Amount,
            dto.TransactionDate,
            dto.Type,
            dto.CategoryId,
            dto.Notes);
    }

    public static void UpdateEntity(
        this FinancialTransaction transaction,
        UpdateFinancialTransactionDto dto)
    {
        transaction.Update(
            dto.Description,
            dto.Amount,
            dto.TransactionDate,
            dto.Type,
            dto.CategoryId,
            dto.Notes);
    }
}