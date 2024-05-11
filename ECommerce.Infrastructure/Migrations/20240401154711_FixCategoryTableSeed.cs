using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryTableSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f63097d2-7f97-44ec-9b30-0526e9b478f3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fdde7e26-1c7a-42e5-b7eb-0acd83284e81"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("71886f11-075c-48bc-b5b0-35a4e68a7c33"), "Fruits" },
                    { new Guid("e5e04394-6595-4680-8ebe-1030523a01dd"), "Vegetables" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("71886f11-075c-48bc-b5b0-35a4e68a7c33"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e5e04394-6595-4680-8ebe-1030523a01dd"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("f63097d2-7f97-44ec-9b30-0526e9b478f3"), "Vegetables" },
                    { new Guid("fdde7e26-1c7a-42e5-b7eb-0acd83284e81"), "Fruits" }
                });
        }
    }
}
