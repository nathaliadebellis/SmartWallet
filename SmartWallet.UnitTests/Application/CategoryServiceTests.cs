using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmartWallet.Application.DTOs.Categories;
using SmartWallet.Application.Services;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Exceptions;
using SmartWallet.Domain.Interfaces;
using Xunit;

namespace SmartWallet.UnitTests.Application;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var categories = new List<Category>
        {
            new Category("Food", TransactionType.Expense, "desc", "icon", "#fff"),
            new Category("Salary", TransactionType.Income, null, null, null)
        };

        repoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(categories);

        var service = new CategoryService(repoMock.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Select(d => d.Name).Should().Contain(new[] { "Food", "Salary" });
        repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenNameExists_ShouldThrowDomainException()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var dto = new CreateCategoryDto { Name = "Food", TransactionType = TransactionType.Expense };

        repoMock.Setup(r => r.ExistsByNameAsync(dto.Name)).ReturnsAsync(true);

        var service = new CategoryService(repoMock.Object);

        // Act
        var act = async () => await service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage("*categoria*");
        repoMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldCallAdd()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var dto = new CreateCategoryDto { Name = "Food", TransactionType = TransactionType.Expense, Description = "d", Icon = "i", Color = "c" };

        repoMock.Setup(r => r.ExistsByNameAsync(dto.Name)).ReturnsAsync(false);

        var service = new CategoryService(repoMock.Object);

        // Act
        await service.CreateAsync(dto);

        // Assert
        repoMock.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == dto.Name && c.TransactionType == dto.TransactionType && c.Description == dto.Description && c.Icon == dto.Icon && c.Color == dto.Color)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var dto = new UpdateCategoryDto { Id = 1, Name = "X", TransactionType = TransactionType.Expense };

        repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((Category?)null);

        var service = new CategoryService(repoMock.Object);

        // Act
        var act = async () => await service.UpdateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenNameExistsForOtherId_ShouldThrowDomainException()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var existing = new Category("Old", TransactionType.Expense);
        var dto = new UpdateCategoryDto { Id = 1, Name = "Other", TransactionType = TransactionType.Expense };

        repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
        repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, dto.Id)).ReturnsAsync(true);

        var service = new CategoryService(repoMock.Object);

        // Act
        var act = async () => await service.UpdateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldCallUpdate()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var existing = new Category("Old", TransactionType.Expense);
        var dto = new UpdateCategoryDto { Id = 1, Name = "New", TransactionType = TransactionType.Income, Description = "d", Icon = "i", Color = "c" };

        repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
        repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, dto.Id)).ReturnsAsync(false);

        var service = new CategoryService(repoMock.Object);

        // Act
        await service.UpdateAsync(dto);

        // Assert
        repoMock.Verify(r => r.UpdateAsync(It.Is<Category>(c => c.Name == dto.Name && c.TransactionType == dto.TransactionType && c.Description == dto.Description && c.Icon == dto.Icon && c.Color == dto.Color)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Category?)null);

        var service = new CategoryService(repoMock.Object);

        // Act
        var act = async () => await service.DeleteAsync(1);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldCallDelete()
    {
        // Arrange
        var repoMock = new Mock<ICategoryRepository>();
        var existing = new Category("Test", TransactionType.Expense);
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        var service = new CategoryService(repoMock.Object);

        // Act
        await service.DeleteAsync(1);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }
}
