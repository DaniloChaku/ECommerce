using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingCartItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("82611e95-6ac2-4336-96c9-86af74e843bd"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("1cfdba08-dda0-4ee9-9cb2-cd9f3a35f760") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1cfdba08-dda0-4ee9-9cb2-cd9f3a35f760"));

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("5f5b44a4-d9b4-4759-b742-ee1413e66a68"), null, "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2"), 0, null, "3314c8a1-bbf3-4237-9c0c-3704e20081e7", "admin@example.com", true, false, null, "Unknown", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAELfzFUNLXeomoPlZuzfFwBUPlIjPmtm0ivs1rSYdnsZiyykKmVA1iam6a3UV5W211Q==", null, false, "ca952a49-7129-4595-a328-1a24778715c8", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2") });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5f5b44a4-d9b4-4759-b742-ee1413e66a68"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("82611e95-6ac2-4336-96c9-86af74e843bd"), null, "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("1cfdba08-dda0-4ee9-9cb2-cd9f3a35f760"), 0, null, "34b0b70e-a518-4d2f-9b7c-ea86c20aa431", "admin@example.com", true, false, null, "Unknown", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEOfOMOgHZ21Tx9AE6prqffCWYe3iVZHlm8KNb2sCT0SqDh8m83PxRcEIirsUnxKIVw==", null, false, "155486ef-53c4-470a-8cff-a8e01dbe3d6d", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("1cfdba08-dda0-4ee9-9cb2-cd9f3a35f760") });
        }
    }
}
