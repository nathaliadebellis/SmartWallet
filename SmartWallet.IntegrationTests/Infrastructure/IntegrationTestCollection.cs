namespace SmartWallet.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<SmartWalletWebApplicationFactory>
{
    public const string Name = "Integration";
}
