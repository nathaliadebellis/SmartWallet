using System.Net;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmartWallet.IntegrationTests.Infrastructure;

namespace SmartWallet.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class AccountTests
{
    private readonly SmartWalletWebApplicationFactory _factory;

    public AccountTests(SmartWalletWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/Dashboard")]
    [InlineData("/Transactions")]
    [InlineData("/Categories")]
    public async Task ProtectedPage_WithoutAuthentication_ShouldRedirectToLogin(string url)
    {
        var client = _factory.CreateAppClient();

        var response = await client.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.AbsolutePath.Should().Be("/Account/Login");
    }

    [Fact]
    public async Task Register_WithValidData_ShouldSignInAndCreateDefaultCategories()
    {
        var client = _factory.CreateAppClient();
        var email = HttpClientExtensions.NewEmail();

        var response = await client.RegisterAsync(email);

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.Should().Be("/Dashboard");

        var dashboard = await client.GetAsync("/Dashboard");

        dashboard.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var dbContext = _factory.CreateDbContext();

        var user = await dbContext.Users.SingleAsync(u => u.Email == email);

        var categories = await dbContext.Categories
            .CountAsync(c => c.ApplicationUserId == user.Id);

        categories.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Register_WithExistingEmail_ShouldNotCreateAnotherUser()
    {
        var email = HttpClientExtensions.NewEmail();

        await _factory.CreateAppClient().RegisterAsync(email);

        var client = _factory.CreateAppClient();

        var response = await client.RegisterAsync(email);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var dbContext = _factory.CreateDbContext();

        var users = await dbContext.Users.CountAsync(u => u.Email == email);

        users.Should().Be(1);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldGrantAccessToDashboard()
    {
        var email = HttpClientExtensions.NewEmail();

        await _factory.CreateAppClient().RegisterAsync(email);

        var client = _factory.CreateAppClient();

        var response = await client.LoginAsync(email);

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.Should().Be("/Dashboard");

        var dashboard = await client.GetAsync("/Dashboard");

        dashboard.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldNotAuthenticate()
    {
        var email = HttpClientExtensions.NewEmail();

        await _factory.CreateAppClient().RegisterAsync(email);

        var client = _factory.CreateAppClient();

        var response = await client.LoginAsync(email, "SenhaErrada@1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.ReadDecodedContentAsync();

        content.Should().Contain("Login inválido");

        var dashboard = await client.GetAsync("/Dashboard");

        dashboard.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Login_WithoutAntiforgeryToken_ShouldReturnBadRequest()
    {
        var client = _factory.CreateAppClient();

        var response = await client.PostAsync("/Account/Login", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["Email"] = HttpClientExtensions.NewEmail(),
                ["Password"] = HttpClientExtensions.DefaultPassword
            }));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
