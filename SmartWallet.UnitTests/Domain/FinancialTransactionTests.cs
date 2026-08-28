using System;
using FluentAssertions;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using Xunit;

namespace SmartWallet.UnitTests.Domain;

public class FinancialTransactionTests
{
    [Fact]
    public void Create_WithValidData_ShouldSetProperties()
    {
        // Arrange
        var description = "Salary";
        decimal amount = 1500.50m;
        var date = new DateTime(2024, 1, 1);
        var type = TransactionType.Income;
        var categoryId = 1;
        string? notes = "Monthly payment";

        // Act
        var tx = new FinancialTransaction(description, amount, date, type, categoryId, notes);

        // Assert
        tx.Description.Should().Be(description);
        tx.Amount.Should().Be(amount);
        tx.TransactionDate.Should().Be(date);
        tx.Type.Should().Be(type);
        tx.CategoryId.Should().Be(categoryId);
        tx.Notes.Should().Be(notes);
        tx.CreatedAt.Should().NotBe(default);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidDescription_ShouldThrow(string? invalid)
    {
        // Act
        Action act = () => new FinancialTransaction(invalid ?? string.Empty, 10m, DateTime.UtcNow, TransactionType.Expense, 1);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Description is required.*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_WithNonPositiveAmount_ShouldThrow(decimal invalidAmount)
    {
        // Act
        Action act = () => new FinancialTransaction("Desc", invalidAmount, DateTime.UtcNow, TransactionType.Expense, 1);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Amount must be greater than zero.*");
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdatePropertiesAndSetUpdatedAt()
    {
        // Arrange
        var tx = new FinancialTransaction("Old", 10m, DateTime.UtcNow, TransactionType.Expense, 1);
        var newDate = DateTime.UtcNow.AddDays(1);

        // Act
        tx.Update("New Description", 20m, newDate, TransactionType.Income, 2, "notes");

        // Assert
        tx.Description.Should().Be("New Description");
        tx.Amount.Should().Be(20m);
        tx.TransactionDate.Should().Be(newDate);
        tx.Type.Should().Be(TransactionType.Income);
        tx.CategoryId.Should().Be(2);
        tx.Notes.Should().Be("notes");
        tx.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public void Update_WithInvalidData_ShouldThrow()
    {
        // Arrange
        var tx = new FinancialTransaction("Old", 10m, DateTime.UtcNow, TransactionType.Expense, 1);

        // Act
        Action act = () => tx.Update("", 0m, DateTime.UtcNow, TransactionType.Expense, 1, null);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
