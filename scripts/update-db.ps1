
dotnet ef migrations add InitialIdentity -p SmartWallet.Infrastructure -s SmartWallet.Web --context ApplicationDbContext

dotnet ef database update -p SmartWallet.Infrastructure -s SmartWallet.Web --context ApplicationDbContext

Write-Host "Migrations added and database updated (if migrations were created)."