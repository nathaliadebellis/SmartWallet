using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWallet.Domain.Entities;

namespace SmartWallet.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder.HasIndex(category => category.Name)
       .IsUnique();

        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(Category.NameMaxLength);

        builder.Property(category => category.Description)
            .HasMaxLength(Category.DescriptionMaxLength);

        builder.Property(category => category.Icon)
            .HasMaxLength(Category.IconMaxLength);

        builder.Property(category => category.Color)
            .HasMaxLength(Category.ColorMaxLength);

        builder.HasData(
            new
            {
                Id = 1,
                Name = "Alimentação",
                Description = "Gastos com alimentação",
                Icon = "bi-basket2",
                Color = "#198754",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 2,
                Name = "Transporte",
                Description = "Gastos com transporte",
                Icon = "bi-car-front",
                Color = "#0D6EFD",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 3,
                Name = "Moradia",
                Description = "Despesas da residência",
                Icon = "bi-house",
                Color = "#6F42C1",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 4,
                Name = "Saúde",
                Description = "Despesas com saúde",
                Icon = "bi-heart-pulse",
                Color = "#DC3545",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 5,
                Name = "Educação",
                Description = "Cursos e estudos",
                Icon = "bi-book",
                Color = "#FD7E14",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 6,
                Name = "Lazer",
                Description = "Entretenimento e lazer",
                Icon = "bi-controller",
                Color = "#20C997",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 7,
                Name = "Salário",
                Description = "Receitas de salário",
                Icon = "bi-cash-stack",
                Color = "#198754",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 8,
                Name = "Investimentos",
                Description = "Aplicações financeiras",
                Icon = "bi-graph-up-arrow",
                Color = "#FFC107",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 9,
                Name = "Mercado",
                Description = "Compras de supermercado",
                Icon = "bi-cart",
                Color = "#6610F2",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 10,
                Name = "Pets",
                Description = "Despesas com animais",
                Icon = "bi-heart",
                Color = "#E83E8C",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 11,
                Name = "Assinaturas",
                Description = "Serviços recorrentes",
                Icon = "bi-credit-card",
                Color = "#6C757D",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 12,
                Name = "Outros",
                Description = "Outras categorias",
                Icon = "bi-three-dots",
                Color = "#343A40",
                CreatedAt = new DateTime(2026, 1, 1)
            });
    }
}