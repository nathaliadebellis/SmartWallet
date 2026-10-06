
dotnet ef database update -p SmartWallet.Infrastructure -s SmartWallet.Web --context ApplicationDbContext

Write-Host "Database update completed (if connection and migrations are valid)."