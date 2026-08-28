using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Services;
using SmartWallet.Application.Mappings;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Exceptions;
using SmartWallet.Domain.Interfaces;
using Xunit;

namespace SmartWallet.UnitTests.Application;

public class FinancialTransactionServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        var transactions = new List<FinancialTransaction>
        {
            new FinancialTransaction("Coffee", 5m, System.DateTime.UtcNow, TransactionType.Expense, 1) { },
            new FinancialTransaction("Salary", 1000m, System.DateTime.UtcNow, TransactionType.Income, 2) { }
        };

        // categories not set here; mapping will return empty CategoryName when null

        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(transactions);

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Select(r => r.Description).Should().Contain(new[] { "Coffee", "Salary" });
        repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenFound_ShouldReturnDto()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        var tx = new FinancialTransaction("Coffee", 5m, System.DateTime.UtcNow, TransactionType.Expense, 1);
        // Category not set here; mapping will use CategoryName = string.Empty

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tx);

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Description.Should().Be("Coffee");
        repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallAdd()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        var dto = new CreateFinancialTransactionDto
        {
            Description = "Test",
            Amount = 10m,
            TransactionDate = System.DateTime.UtcNow,
            Type = TransactionType.Expense,
            CategoryId = 1,
            Notes = "n"
        };

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        await service.CreateAsync(dto);

        // Assert
        repoMock.Verify(r => r.AddAsync(It.Is<FinancialTransaction>(t => t.Description == dto.Description && t.Amount == dto.Amount && t.CategoryId == dto.CategoryId && t.Notes == dto.Notes)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        var dto = new UpdateFinancialTransactionDto { Id = 1, Description = "X", Amount = 5m, TransactionDate = System.DateTime.UtcNow, Type = TransactionType.Expense, CategoryId = 1 };

        repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((FinancialTransaction?)null);

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        var act = async () => await service.UpdateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldCallUpdate()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        var existing = new FinancialTransaction("Old", 1m, System.DateTime.UtcNow, TransactionType.Expense, 1);
        var dto = new UpdateFinancialTransactionDto { Id = 1, Description = "New", Amount = 20m, TransactionDate = System.DateTime.UtcNow, Type = TransactionType.Income, CategoryId = 2, Notes = "n" };

        repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        await service.UpdateAsync(dto);

        // Assert
        repoMock.Verify(r => r.UpdateAsync(It.Is<FinancialTransaction>(t => t.Description == dto.Description && t.Amount == dto.Amount && t.CategoryId == dto.CategoryId && t.Notes == dto.Notes)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((FinancialTransaction?)null);

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        var act = async () => await service.DeleteAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldCallDelete()
    {
        // Arrange
        var repoMock = new Mock<IFinancialTransactionRepository>();
        var existing = new FinancialTransaction("T", 1m, System.DateTime.UtcNow, TransactionType.Expense, 1);
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        var service = new FinancialTransactionService(repoMock.Object);

        // Act
        await service.DeleteAsync(1);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }
}
