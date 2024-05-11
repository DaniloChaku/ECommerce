using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fc839d07-4ece-43da-8377-4b82110d5651"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("fc839d07-4ece-43da-8377-4b82110d5651"), null, "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e"), 0, "6703a3fc-f9c2-4b5f-b131-30410e4d49cf", "admin@example.com", true, false, null, "Unknown", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEH0z24HTY/ip5gPB/sg8zs5hatXJGltfonGzCQrMjEIXMQsXm0oNDOd/nAo3+LMyRg==", null, false, "53183e33-0bf0-489a-b659-3a4a8260a968", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e") });
        }
    }
}
