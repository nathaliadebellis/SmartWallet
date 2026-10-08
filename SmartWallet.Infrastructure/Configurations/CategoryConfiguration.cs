using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallet.Domain.Entities;
using SmartWallet.Infrastructure.Identity;

namespace SmartWallet.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(Category.NameMaxLength);

        builder.Property(c => c.ApplicationUserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.ApplicationUserId, c.Name })
            .IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(Category.DescriptionMaxLength);

        builder.Property(c => c.Icon)
            .HasMaxLength(Category.IconMaxLength);

        builder.Property(c => c.Color)
            .HasMaxLength(Category.ColorMaxLength);

        builder.Property(c => c.TransactionType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}