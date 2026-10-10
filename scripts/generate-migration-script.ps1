# Generates an idempotent SQL script with all migrations in scripts/migrations.sql.
# Use it to update a database where the application is not allowed to apply migrations itself.
# Run from the solution root.
dotnet tool restore
dotnet ef migrations script --idempotent -p SmartWallet.Infrastructure -s SmartWallet.Web --context ApplicationDbContext -o scripts/migrations.sql

Write-Host "Migration script generated in scripts/migrations.sql."
