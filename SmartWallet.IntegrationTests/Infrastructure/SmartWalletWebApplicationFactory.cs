using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Infrastructure.Data;

namespace SmartWallet.IntegrationTests.Infrastructure;

/// <summary>
/// Sobe a aplicação real contra um banco SQL Server exclusivo da execução de
/// testes. O banco é criado pelas migrations na inicialização e removido ao final.
/// </summary>
public class SmartWalletWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionEnvironmentVariable = "SMARTWALLET_TEST_CONNECTION";

    private const string DefaultConnection =
        "Server=(localdb)\\MSSQLLocalDB;Trusted_Connection=True;TrustServerCertificate=True";

    private readonly string _connectionString;

    public SmartWalletWebApplicationFactory()
    {
        var baseConnection = Environment.GetEnvironmentVariable(ConnectionEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(baseConnection))
            baseConnection = DefaultConnection;

        _connectionString = new SqlConnectionStringBuilder(baseConnection)
        {
            InitialCatalog = $"SmartWalletTests_{Guid.NewGuid():N}"
        }.ConnectionString;
    }

    public HttpClient CreateAppClient()
    {
        // O cookie de autenticação é Secure, então o cliente precisa usar HTTPS.
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false,
            HandleCookies = true
        });
    }

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _connectionString);
        builder.UseSetting("Seed:AdminPassword", string.Empty);
        builder.UseSetting("Email:Smtp:Host", string.Empty);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        using var dbContext = CreateDbContext();

        dbContext.Database.EnsureDeleted();
    }
}
