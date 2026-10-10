using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Domain.Enums;
using SmartWallet.IntegrationTests.Infrastructure;

namespace SmartWallet.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class TransactionIsolationTests
{
    private readonly SmartWalletWebApplicationFactory _factory;

    public TransactionIsolationTests(SmartWalletWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Create_WithValidData_ShouldPersistTransactionForLoggedUser()
    {
        var (client, email) = await CreateAuthenticatedClientAsync();

        var description = NewDescription();

        var transactionId = await CreateTransactionAsync(client, email, description);

        await using var dbContext = _factory.CreateDbContext();

        var user = await dbContext.Users.SingleAsync(u => u.Email == email);

        var transaction = await dbContext.FinancialTransactions.SingleAsync(t => t.Id == transactionId);

        transaction.ApplicationUserId.Should().Be(user.Id);
        transaction.Amount.Should().Be(150.75m);
        transaction.Type.Should().Be(TransactionType.Expense);
    }

    [Fact]
    public async Task Index_ShouldShowOnlyTransactionsOfLoggedUser()
    {
        var (ownerClient, ownerEmail) = await CreateAuthenticatedClientAsync();
        var (otherClient, _) = await CreateAuthenticatedClientAsync();

        var description = NewDescription();

        await CreateTransactionAsync(ownerClient, ownerEmail, description);

        var ownerList = await ownerClient.GetDecodedStringAsync("/Transactions");
        var otherList = await otherClient.GetDecodedStringAsync("/Transactions");
        var otherDashboard = await otherClient.GetDecodedStringAsync("/Dashboard");

        ownerList.Should().Contain(description);
        otherList.Should().NotContain(description);
        otherDashboard.Should().NotContain(description);
    }

    [Theory]
    [InlineData("/Transactions/Edit/")]
    [InlineData("/Transactions/Delete/")]
    public async Task Get_TransactionOfAnotherUser_ShouldReturnNotFound(string url)
    {
        var (ownerClient, ownerEmail) = await CreateAuthenticatedClientAsync();
        var (otherClient, _) = await CreateAuthenticatedClientAsync();

        var transactionId = await CreateTransactionAsync(ownerClient, ownerEmail, NewDescription());

        var ownerResponse = await ownerClient.GetAsync(url + transactionId);
        var otherResponse = await otherClient.GetAsync(url + transactionId);

        ownerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        otherResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_TransactionOfAnotherUser_ShouldReturnNotFoundAndKeepTransaction()
    {
        var (ownerClient, ownerEmail) = await CreateAuthenticatedClientAsync();
        var (otherClient, _) = await CreateAuthenticatedClientAsync();

        var transactionId = await CreateTransactionAsync(ownerClient, ownerEmail, NewDescription());

        var response = await otherClient.PostFormAsync(
            "/Transactions/Create",
            $"/Transactions/Delete/{transactionId}",
            new Dictionary<string, string>());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var dbContext = _factory.CreateDbContext();

        var exists = await dbContext.FinancialTransactions.AnyAsync(t => t.Id == transactionId);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task Edit_TransactionOfAnotherUser_ShouldReturnNotFoundAndKeepData()
    {
        var (ownerClient, ownerEmail) = await CreateAuthenticatedClientAsync();
        var (otherClient, otherEmail) = await CreateAuthenticatedClientAsync();

        var description = NewDescription();

        var transactionId = await CreateTransactionAsync(ownerClient, ownerEmail, description);

        var response = await otherClient.PostFormAsync(
            "/Transactions/Create",
            "/Transactions/Edit",
            new Dictionary<string, string>
            {
                ["Id"] = transactionId.ToString(),
                ["Description"] = "Alterada por outro usuário",
                ["Amount"] = "1.00",
                ["TransactionDate"] = "2026-10-01",
                ["Type"] = nameof(TransactionType.Expense),
                ["CategoryId"] = (await GetExpenseCategoryIdAsync(otherEmail)).ToString()
            });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var dbContext = _factory.CreateDbContext();

        var transaction = await dbContext.FinancialTransactions.SingleAsync(t => t.Id == transactionId);

        transaction.Description.Should().Be(description);
    }

    private static string NewDescription() => $"Transação {Guid.NewGuid():N}";

    private async Task<(HttpClient Client, string Email)> CreateAuthenticatedClientAsync()
    {
        var client = _factory.CreateAppClient();
        var email = HttpClientExtensions.NewEmail();

        var response = await client.RegisterAsync(email);

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        return (client, email);
    }

    private async Task<int> GetExpenseCategoryIdAsync(string email)
    {
        await using var dbContext = _factory.CreateDbContext();

        var user = await dbContext.Users.SingleAsync(u => u.Email == email);

        return await dbContext.Categories
            .Where(c => c.ApplicationUserId == user.Id && c.TransactionType == TransactionType.Expense)
            .Select(c => c.Id)
            .FirstAsync();
    }

    private async Task<int> CreateTransactionAsync(HttpClient client, string email, string description)
    {
        var response = await client.PostFormAsync(
            "/Transactions/Create",
            "/Transactions/Create",
            new Dictionary<string, string>
            {
                ["Description"] = description,
                ["Amount"] = "150.75",
                ["TransactionDate"] = "2026-10-01",
                ["Type"] = nameof(TransactionType.Expense),
                ["CategoryId"] = (await GetExpenseCategoryIdAsync(email)).ToString()
            });

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        await using var dbContext = _factory.CreateDbContext();

        return await dbContext.FinancialTransactions
            .Where(t => t.Description == description)
            .Select(t => t.Id)
            .SingleAsync();
    }
}
