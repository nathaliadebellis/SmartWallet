using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Application.Interfaces;
using SmartWallet.Application.Services;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Exceptions;
using SmartWallet.Domain.Interfaces;
using Xunit;

namespace SmartWallet.UnitTests.Application;

public class CategoryServiceTests
{
    private const string UserId = "test-user";

    private readonly Mock<ICategoryRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();

    private CategoryService CreateService() => new(_repoMock.Object, _uowMock.Object);

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category("Food", TransactionType.Expense, "desc", "icon", "#fff"),
            new Category("Salary", TransactionType.Income, null, null, null)
        };

        _repoMock.Setup(r => r.GetAllAsync(UserId)).ReturnsAsync(categories);

        // Act
        var result = await CreateService().GetAllAsync(UserId);

        // Assert
        result.Should().HaveCount(2);
        result.Select(d => d.Name).Should().Contain(new[] { "Food", "Salary" });
        _repoMock.Verify(r => r.GetAllAsync(UserId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenNameExists_ShouldThrowDomainException()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "Food", TransactionType = TransactionType.Expense };

        _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, UserId)).ReturnsAsync(true);

        // Act
        var act = async () => await CreateService().CreateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage("*categoria*");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldCallAddWithOwner()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "Food", TransactionType = TransactionType.Expense, Description = "d", Icon = "i", Color = "c" };

        _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, UserId)).ReturnsAsync(false);

        // Act
        await CreateService().CreateAsync(dto, UserId);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.Is<Category>(c =>
            c.Name == dto.Name &&
            c.TransactionType == dto.TransactionType &&
            c.Description == dto.Description &&
            c.Icon == dto.Icon &&
            c.Color == dto.Color &&
            c.ApplicationUserId == UserId)), Times.Once);
    }

    [Fact]
    public async Task CreateDefaultCategoriesAsync_ShouldAddCategoriesForTheUser()
    {
        // Arrange
        IEnumerable<Category>? added = null;

        _repoMock.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Category>>()))
            .Callback<IEnumerable<Category>>(c => added = c.ToList())
            .Returns(Task.CompletedTask);

        // Act
        await CreateService().CreateDefaultCategoriesAsync(UserId);

        // Assert
        added.Should().NotBeNullOrEmpty();
        added!.Should().OnlyContain(c => c.ApplicationUserId == UserId);
        added!.Select(c => c.TransactionType).Should().Contain(new[] { TransactionType.Income, TransactionType.Expense });
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var dto = new UpdateCategoryDto { Id = 1, Name = "X", TransactionType = TransactionType.Expense };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id, UserId)).ReturnsAsync((Category?)null);

        // Act
        var act = async () => await CreateService().UpdateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenNameExistsForOtherId_ShouldThrowDomainException()
    {
        // Arrange
        var existing = new Category("Old", TransactionType.Expense);
        var dto = new UpdateCategoryDto { Id = 1, Name = "Other", TransactionType = TransactionType.Expense };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id, UserId)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, UserId, dto.Id)).ReturnsAsync(true);

        // Act
        var act = async () => await CreateService().UpdateAsync(dto, UserId);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldCallUpdate()
    {
        // Arrange
        var existing = new Category("Old", TransactionType.Expense);
        var dto = new UpdateCategoryDto { Id = 1, Name = "New", TransactionType = TransactionType.Income, Description = "d", Icon = "i", Color = "c" };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id, UserId)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, UserId, dto.Id)).ReturnsAsync(false);

        // Act
        await CreateService().UpdateAsync(dto, UserId);

        // Assert
        _repoMock.Verify(r => r.UpdateAsync(It.Is<Category>(c => c.Name == dto.Name && c.TransactionType == dto.TransactionType && c.Description == dto.Description && c.Icon == dto.Icon && c.Color == dto.Color)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1, UserId)).ReturnsAsync((Category?)null);

        // Act
        var act = async () => await CreateService().DeleteAsync(1, UserId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryHasTransactions_ShouldThrowDomainException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1, UserId)).ReturnsAsync(new Category("Test", TransactionType.Expense));
        _repoMock.Setup(r => r.HasTransactionsAsync(1, UserId)).ReturnsAsync(true);

        // Act
        var act = async () => await CreateService().DeleteAsync(1, UserId);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldCallDelete()
    {
        // Arrange
        var existing = new Category("Test", TransactionType.Expense);
        _repoMock.Setup(r => r.GetByIdAsync(1, UserId)).ReturnsAsync(existing);

        // Act
        await CreateService().DeleteAsync(1, UserId);

        // Assert
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }
}
