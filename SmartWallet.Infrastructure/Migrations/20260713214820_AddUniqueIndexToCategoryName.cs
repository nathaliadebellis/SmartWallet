using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToCategoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
