using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmartWallet.Application.DTOs.FinancialTransactions;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Services;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Exceptions;
using SmartWallet.Domain.Interfaces;
using Xunit;

namespace SmartWallet.UnitTests.Application;

public class FinancialTransactionServiceTests
{
    private const string UserId = "test-user";
    private const string OtherUserId = "other-user";

    private readonly Mock<IFinancialTransactionRepository> _repoMock = new();
    private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();

    private FinancialTransactionService CreateService() =>
        new(_repoMock.Object, _categoryRepoMock.Object, _uowMock.Object);

    private void SetupCategory(int id, TransactionType type, string userId = UserId)
    {
        _categoryRepoMock.Setup(r => r.GetByIdAsync(id, userId))
            .ReturnsAsync(new Category("Cat", type) { ApplicationUserId = userId });
    }

    private static FinancialTransaction NewTransaction(string description, string userId = UserId)
    {
        return new FinancialTransaction(description, 5m, System.DateTime.UtcNow, TransactionType.Expense, 1)
        {
            ApplicationUserId = userId
        };
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyTransactionsOfTheUser()
    {
        // Arrange
        var transactions = new List<FinancialTransaction>
        {
            NewTransaction("Coffee"),
            NewTransaction("Salary")
        };

        _repoMock.Setup(r => r.GetByUserAsync(UserId)).ReturnsAsync(transactions);

        // Act
        var result = await CreateService().GetAllAsync(UserId);

        // Assert
        result.Should().HaveCount(2);
        result.Select(r => r.Description).Should().Contain(new[] { "Coffee", "Salary" });
        _repoMock.Verify(r => r.GetByUserAsync(UserId), Times.Once);
        _repoMock.Verify(r => r.GetByUserAsync(OtherUserId), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WhenFound_ShouldReturnDto()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(NewTransaction("Coffee"));

        // Act
        var result = await CreateService().GetByIdAsync(1, UserId);

        // Assert
        result.Should().NotBeNull();
        result!.Description.Should().Be("Coffee");
    }

    [Fact]
    public async Task GetByIdAsync_WhenBelongsToAnotherUser_ShouldReturnNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(NewTransaction("Coffee", OtherUserId));

        // Act
        var result = await CreateService().GetByIdAsync(1, UserId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCallAddWithOwner()
    {
        // Arrange
        SetupCategory(1, TransactionType.Expense);

        var dto = new CreateFinancialTransactionDto
        {
            Description = "Test",
            Amount = 10m,
            TransactionDate = System.DateTime.UtcNow,
            Type = TransactionType.Expense,
            CategoryId = 1,
            Notes = "n"
        };

        // Act
        await CreateService().CreateAsync(dto, UserId);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.Is<FinancialTransaction>(t =>
            t.Description == dto.Description &&
            t.Amount == dto.Amount &&
            t.CategoryId == dto.CategoryId &&
            t.Notes == dto.Notes &&
            t.ApplicationUserId == UserId)), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenCategoryBelongsToAnotherUser_ShouldThrowDomainException()
    {
        // Arrange: category 1 exists only for the other user
        SetupCategory(1, TransactionType.Expense, OtherUserId);

        var dto = new CreateFinancialTransactionDto
        {
            Description = "Test",
            Amount = 10m,
            TransactionDate = System.DateTime.UtcNow,
            Type = TransactionType.Expense,
            CategoryId = 1
        };

        // Act
        var act = async () => await CreateService().CreateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCategoryTypeDiffers_ShouldThrowDomainException()
    {
        // Arrange
        SetupCategory(1, TransactionType.Income);

        var dto = new CreateFinancialTransactionDto
        {
            Description = "Test",
            Amount = 10m,
            TransactionDate = System.DateTime.UtcNow,
            Type = TransactionType.Expense,
            CategoryId = 1
        };

        // Act
        var act = async () => await CreateService().CreateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var dto = new UpdateFinancialTransactionDto { Id = 1, Description = "X", Amount = 5m, TransactionDate = System.DateTime.UtcNow, Type = TransactionType.Expense, CategoryId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((FinancialTransaction?)null);

        // Act
        var act = async () => await CreateService().UpdateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenBelongsToAnotherUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var dto = new UpdateFinancialTransactionDto { Id = 1, Description = "X", Amount = 5m, TransactionDate = System.DateTime.UtcNow, Type = TransactionType.Expense, CategoryId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(NewTransaction("Old", OtherUserId));
        SetupCategory(1, TransactionType.Expense);

        // Act
        var act = async () => await CreateService().UpdateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldCallUpdate()
    {
        // Arrange
        var dto = new UpdateFinancialTransactionDto { Id = 1, Description = "New", Amount = 20m, TransactionDate = System.DateTime.UtcNow, Type = TransactionType.Income, CategoryId = 2, Notes = "n" };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(NewTransaction("Old"));
        SetupCategory(2, TransactionType.Income);

        // Act
        await CreateService().UpdateAsync(dto, UserId);

        // Assert
        _repoMock.Verify(r => r.UpdateAsync(It.Is<FinancialTransaction>(t => t.Description == dto.Description && t.Amount == dto.Amount && t.CategoryId == dto.CategoryId && t.Notes == dto.Notes)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((FinancialTransaction?)null);

        // Act
        var act = async () => await CreateService().DeleteAsync(1, UserId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenBelongsToAnotherUser_ShouldThrowNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(NewTransaction("T", OtherUserId));

        // Act
        var act = async () => await CreateService().DeleteAsync(1, UserId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<FinancialTransaction>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldCallDelete()
    {
        // Arrange
        var existing = NewTransaction("T");
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        // Act
        await CreateService().DeleteAsync(1, UserId);

        // Assert
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }
}
