using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "#198754", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gastos com alimentação", "bi-basket2", "Alimentação", null },
                    { 2, "#0D6EFD", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gastos com transporte", "bi-car-front", "Transporte", null },
                    { 3, "#6F42C1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Despesas da residência", "bi-house", "Moradia", null },
                    { 4, "#DC3545", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Despesas com saúde", "bi-heart-pulse", "Saúde", null },
                    { 5, "#FD7E14", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cursos e estudos", "bi-book", "Educação", null },
                    { 6, "#20C997", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Entretenimento e lazer", "bi-controller", "Lazer", null },
                    { 7, "#198754", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Receitas de salário", "bi-cash-stack", "Salário", null },
                    { 8, "#FFC107", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aplicações financeiras", "bi-graph-up-arrow", "Investimentos", null },
                    { 9, "#6610F2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compras de supermercado", "bi-cart", "Mercado", null },
                    { 10, "#E83E8C", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Despesas com animais", "bi-heart", "Pets", null },
                    { 11, "#6C757D", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Serviços recorrentes", "bi-credit-card", "Assinaturas", null },
                    { 12, "#343A40", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Outras categorias", "bi-three-dots", "Outros", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_CategoryId",
                table: "FinancialTransactions",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialTransactions");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
