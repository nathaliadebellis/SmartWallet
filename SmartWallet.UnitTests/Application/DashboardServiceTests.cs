using FluentAssertions;
using Moq;
using SmartWallet.Application.Services;
using SmartWallet.Domain.Entities;
using SmartWallet.Domain.Enums;
using SmartWallet.Domain.Interfaces;
using SmartWallet.Domain.Reports;

namespace SmartWallet.UnitTests.Application;

public class DashboardServiceTests
{
    private const string UserId = "test-user";

    private readonly Mock<IFinancialTransactionRepository> _repoMock = new();

    private static DateTime CurrentMonth => new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public DashboardServiceTests()
    {
        _repoMock.Setup(r => r.GetLatestByUserAsync(UserId, It.IsAny<int>()))
            .ReturnsAsync(new List<FinancialTransaction>());

        SetupCategoryTotals();
        SetupMonthlyTotals();
    }

    private DashboardService CreateService() => new(_repoMock.Object);

    private void SetupCategoryTotals(params CategoryTotal[] totals)
    {
        _repoMock.Setup(r => r.GetExpenseTotalsByCategoryAsync(UserId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(totals);
    }

    private void SetupMonthlyTotals(params MonthlyTotal[] totals)
    {
        _repoMock.Setup(r => r.GetMonthlyTotalsAsync(UserId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(totals);
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldCalculateBalanceFromTotals()
    {
        _repoMock.Setup(r => r.GetTotalByUserAndTypeAsync(UserId, TransactionType.Income)).ReturnsAsync(1000m);
        _repoMock.Setup(r => r.GetTotalByUserAndTypeAsync(UserId, TransactionType.Expense)).ReturnsAsync(350.50m);

        var result = await CreateService().GetDashboardAsync(UserId);

        result.TotalIncome.Should().Be(1000m);
        result.TotalExpenses.Should().Be(350.50m);
        result.CurrentBalance.Should().Be(649.50m);
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldRequestCategoryTotalsForCurrentMonth()
    {
        await CreateService().GetDashboardAsync(UserId);

        _repoMock.Verify(r => r.GetExpenseTotalsByCategoryAsync(
            UserId,
            CurrentMonth,
            CurrentMonth.AddMonths(1)));
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldOrderCategoriesByTotalDescending()
    {
        SetupCategoryTotals(
            new CategoryTotal("Lazer", 50m),
            new CategoryTotal("Moradia", 900m),
            new CategoryTotal("Transporte", 200m));

        var result = await CreateService().GetDashboardAsync(UserId);

        result.ExpensesByCategory.Select(c => c.CategoryName)
            .Should().Equal("Moradia", "Transporte", "Lazer");
    }

    [Fact]
    public async Task GetDashboardAsync_WithManyCategories_ShouldGroupSmallestIntoOthers()
    {
        SetupCategoryTotals(Enumerable.Range(1, 8)
            .Select(i => new CategoryTotal($"Categoria {i}", i * 10m))
            .ToArray());

        var result = await CreateService().GetDashboardAsync(UserId);

        result.ExpensesByCategory.Should().HaveCount(DashboardService.MaxCategoriesInChart);

        var others = result.ExpensesByCategory.Last();

        others.CategoryName.Should().Be(DashboardService.OtherCategoriesName);
        others.Total.Should().Be(10m + 20m + 30m);

        result.ExpensesByCategory.Sum(c => c.Total).Should().Be(360m);
    }

    [Fact]
    public async Task GetDashboardAsync_ShouldReturnLastSixMonthsFillingEmptyOnesWithZero()
    {
        var previousMonth = CurrentMonth.AddMonths(-1);

        SetupMonthlyTotals(
            new MonthlyTotal(CurrentMonth.Year, CurrentMonth.Month, TransactionType.Income, 3000m),
            new MonthlyTotal(CurrentMonth.Year, CurrentMonth.Month, TransactionType.Expense, 1200m),
            new MonthlyTotal(previousMonth.Year, previousMonth.Month, TransactionType.Expense, 800m));

        var result = await CreateService().GetDashboardAsync(UserId);

        var firstMonth = CurrentMonth.AddMonths(-(DashboardService.MonthsInSummary - 1));

        result.MonthlySummary.Should().HaveCount(DashboardService.MonthsInSummary);

        result.MonthlySummary.First().Year.Should().Be(firstMonth.Year);
        result.MonthlySummary.First().Month.Should().Be(firstMonth.Month);
        result.MonthlySummary.First().Income.Should().Be(0m);
        result.MonthlySummary.First().Expenses.Should().Be(0m);

        var current = result.MonthlySummary.Last();

        current.Month.Should().Be(CurrentMonth.Month);
        current.Income.Should().Be(3000m);
        current.Expenses.Should().Be(1200m);

        var previous = result.MonthlySummary[^2];

        previous.Income.Should().Be(0m);
        previous.Expenses.Should().Be(800m);
    }
}
