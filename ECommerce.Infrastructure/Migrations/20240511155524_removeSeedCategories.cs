using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeSeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5f5b44a4-d9b4-4759-b742-ee1413e66a68"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2") });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("71886f11-075c-48bc-b5b0-35a4e68a7c33"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e5e04394-6595-4680-8ebe-1030523a01dd"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("5a598257-8e8b-4d63-bc64-d1068ce37f58"), null, "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("369b6eed-1926-4ea7-a3e0-73810ed9af19"), 0, null, "e7c1b482-c7a4-47db-ad5a-aff4059bcd96", "admin@example.com", true, false, null, "Unknown", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAECx7eQ6p2daZLekT6o0VtbEmV4ITcPj6fzkWCuV1wbmdWT7CCIbs30uHExCVAgonUQ==", null, false, "1995a329-7b66-406b-bc5c-3840d34b25aa", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("369b6eed-1926-4ea7-a3e0-73810ed9af19") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5a598257-8e8b-4d63-bc64-d1068ce37f58"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("369b6eed-1926-4ea7-a3e0-73810ed9af19") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("369b6eed-1926-4ea7-a3e0-73810ed9af19"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("5f5b44a4-d9b4-4759-b742-ee1413e66a68"), null, "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2"), 0, null, "3314c8a1-bbf3-4237-9c0c-3704e20081e7", "admin@example.com", true, false, null, "Unknown", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAELfzFUNLXeomoPlZuzfFwBUPlIjPmtm0ivs1rSYdnsZiyykKmVA1iam6a3UV5W211Q==", null, false, "ca952a49-7129-4595-a328-1a24778715c8", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("71886f11-075c-48bc-b5b0-35a4e68a7c33"), "Fruits" },
                    { new Guid("e5e04394-6595-4680-8ebe-1030523a01dd"), "Vegetables" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("d055a00c-5cc8-4318-8bd8-3d70b1375bc2") });
        }
    }
}
