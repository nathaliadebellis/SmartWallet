using System;
using FluentAssertions;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using Xunit;

namespace SmartWallet.UnitTests.Domain;

public class CategoryTests
{
    [Fact]
    public void Create_WithValidData_ShouldSetProperties()
    {
        var name = "  Food  ";
        var transactionType = TransactionType.Expense;
        var description = "Groceries and dining";
        var icon = "fa-utensils";
        var color = "#ff0000";

        var category = new Category(name, transactionType, description, icon, color);

        category.Name.Should().Be("Food"); // nome deve ser trimado
        category.TransactionType.Should().Be(transactionType);
        category.Description.Should().Be(description);
        category.Icon.Should().Be(icon);
        category.Color.Should().Be(color);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldThrow(string? invalidName)
    {

        Action act = () => new Category(invalidName ?? string.Empty, TransactionType.Expense);

        act.Should().Throw<ArgumentException>().WithMessage("Nome da categoria é obrigatório.*");
    }

    [Fact]
    public void Create_WithTooLongName_ShouldThrow()
    {
        var longName = new string('a', Category.NameMaxLength + 1);

        Action act = () => new Category(longName, TransactionType.Income);

        act.Should().Throw<ArgumentException>().WithMessage($"O nome da categoria não pode exceder {Category.NameMaxLength} caracteres.*");
    }

    [Fact]
    public void ChangeDescription_WithTooLongDescription_ShouldThrow()
    {
        var category = new Category("Test", TransactionType.Income);
        var longDescription = new string('d', Category.DescriptionMaxLength + 1);

        Action act = () => category.ChangeDescription(longDescription);

        act.Should().Throw<ArgumentException>().WithMessage($"A descrição não pode exceder {Category.DescriptionMaxLength} caracteres.*");
    }

    [Fact]
    public void Update_ShouldChangeProperties()
    {
        var category = new Category("Old", TransactionType.Expense);

        category.Update("New Name", TransactionType.Income, "desc", "icon", "color");

        category.Name.Should().Be("New Name");
        category.TransactionType.Should().Be(TransactionType.Income);
        category.Description.Should().Be("desc");
        category.Icon.Should().Be("icon");
        category.Color.Should().Be("color");
    }
}
