using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionTypeToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Expense");

            migrationBuilder.Sql("""
                UPDATE Categories
                SET TransactionType = 'Income'
                WHERE Name IN ('Salário', 'Investimentos');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Categories");
        }
    }
}