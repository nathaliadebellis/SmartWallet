# Applies existing migrations (creates the database if it does not exist).
# To create a new migration: dotnet ef migrations add <Name> -p SmartWallet.Infrastructure -s SmartWallet.Web
dotnet tool restore
dotnet ef database update -p SmartWallet.Infrastructure -s SmartWallet.Web --context ApplicationDbContext

Write-Host "Database update completed."
