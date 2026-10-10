using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Domain.Enums;
using SmartWallet.IntegrationTests.Infrastructure;

namespace SmartWallet.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class DashboardTests
{
    private readonly SmartWalletWebApplicationFactory _factory;

    public DashboardTests(SmartWalletWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Index_WithoutTransactions_ShouldShowEmptyChartMessages()
    {
        var client = _factory.CreateAppClient();

        await client.RegisterAsync(HttpClientExtensions.NewEmail());

        var dashboard = await client.GetDecodedStringAsync("/Dashboard");

        dashboard.Should().Contain("Nenhuma despesa registrada neste mês.");
        dashboard.Should().Contain("Nenhuma transação registrada neste período.");
        dashboard.Should().NotContain("<canvas");
    }

    [Fact]
    public async Task Index_WithExpenseInCurrentMonth_ShouldRenderChartsWithCategoryTotal()
    {
        var client = _factory.CreateAppClient();
        var email = HttpClientExtensions.NewEmail();

        await client.RegisterAsync(email);

        await using var dbContext = _factory.CreateDbContext();

        var user = await dbContext.Users.SingleAsync(u => u.Email == email);

        var category = await dbContext.Categories
            .FirstAsync(c => c.ApplicationUserId == user.Id && c.TransactionType == TransactionType.Expense);

        foreach (var amount in new[] { "100.00", "50.25" })
        {
            var response = await client.PostFormAsync(
                "/Transactions/Create",
                "/Transactions/Create",
                new Dictionary<string, string>
                {
                    ["Description"] = "Despesa do mês",
                    ["Amount"] = amount,
                    ["TransactionDate"] = DateTime.Today.ToString("yyyy-MM-dd"),
                    ["Type"] = nameof(TransactionType.Expense),
                    ["CategoryId"] = category.Id.ToString()
                });

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        var dashboard = await client.GetDecodedStringAsync("/Dashboard");

        dashboard.Should().Contain("id=\"expensesByCategoryChart\"");
        dashboard.Should().Contain("id=\"monthlySummaryChart\"");
        dashboard.Should().Contain("\"total\":150.25");
    }
}
