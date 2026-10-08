using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallet.Domain.Entities;
using SmartWallet.Infrastructure.Identity;

namespace SmartWallet.Infrastructure.Configurations;

public class FinancialTransactionConfiguration
    : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.ToTable("FinancialTransactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Description)
            .IsRequired()
            .HasMaxLength(FinancialTransaction.DescriptionMaxLength);

        builder.Property(transaction => transaction.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(transaction => transaction.TransactionDate)
            .IsRequired();

        builder.Property(transaction => transaction.Type)
            .IsRequired();

        builder.Property(transaction => transaction.Notes)
            .HasMaxLength(FinancialTransaction.NotesMaxLength);

        #region Relationship

        builder.HasOne(transaction => transaction.Category)
            .WithMany(category => category.Transactions)
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(transaction => transaction.ApplicationUserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(transaction => transaction.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(transaction => new { transaction.ApplicationUserId, transaction.TransactionDate });

        #endregion
    }
}