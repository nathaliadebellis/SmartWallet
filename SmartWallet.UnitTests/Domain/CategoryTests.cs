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
        // Arrange
        var name = "  Food  ";
        var transactionType = TransactionType.Expense;
        var description = "Groceries and dining";
        var icon = "fa-utensils";
        var color = "#ff0000";

        // Act
        var category = new Category(name, transactionType, description, icon, color);

        // Assert
        category.Name.Should().Be("Food"); // trimmed
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
        // Arrange

        // Act
        Action act = () => new Category(invalidName ?? string.Empty, TransactionType.Expense);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Category name is required.*");
    }

    [Fact]
    public void Create_WithTooLongName_ShouldThrow()
    {
        // Arrange
        var longName = new string('a', Category.NameMaxLength + 1);

        // Act
        Action act = () => new Category(longName, TransactionType.Income);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage($"Category name cannot exceed {Category.NameMaxLength} characters.*");
    }

    [Fact]
    public void ChangeDescription_WithTooLongDescription_ShouldThrow()
    {
        // Arrange
        var category = new Category("Test", TransactionType.Income);
        var longDescription = new string('d', Category.DescriptionMaxLength + 1);

        // Act
        Action act = () => category.ChangeDescription(longDescription);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage($"Description cannot exceed {Category.DescriptionMaxLength} characters.*");
    }

    [Fact]
    public void Update_ShouldChangeProperties()
    {
        // Arrange
        var category = new Category("Old", TransactionType.Expense);

        // Act
        category.Update("New Name", TransactionType.Income, "desc", "icon", "color");

        // Assert
        category.Name.Should().Be("New Name");
        category.TransactionType.Should().Be(TransactionType.Income);
        category.Description.Should().Be("desc");
        category.Icon.Should().Be("icon");
        category.Color.Should().Be("color");
    }
}
