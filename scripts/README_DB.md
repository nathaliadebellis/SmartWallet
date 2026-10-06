Usage notes

1) Development using LocalDB (recommended for local dev)
   - Ensure SQL Server Express LocalDB is installed.
   - Connection string is set to (localdb)\\MSSQLLocalDB in SmartWallet.Web/appsettings.json.

2) Production / IIS
   - Do NOT use LocalDB under IIS. Change DefaultConnection to a SQL Server or SQL Express instance (Server=.\\SQLEXPRESS or remote Server=yourserver;)

3) Running migrations
   - From solution root run:
	 powershell.exe -File .\\scripts\\update-db.ps1
   - Or run the dotnet ef commands directly as described in the script.

4) Common troubleshooting
   - If LocalDB automatic instance creation fails under IIS, change to SQLEXPRESS or a proper SQL Server and update appsettings.
   - Ensure migrations assembly in Program.cs is set to SmartWallet.Infrastructure.

If you want, I can change the DefaultConnection to use SQLEXPRESS instead of LocalDB.