using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataIntoCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("f63097d2-7f97-44ec-9b30-0526e9b478f3"), "Vegetables" },
                    { new Guid("fdde7e26-1c7a-42e5-b7eb-0acd83284e81"), "Fruits" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f63097d2-7f97-44ec-9b30-0526e9b478f3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fdde7e26-1c7a-42e5-b7eb-0acd83284e81"));
        }
    }
}
