using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRolesAndAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), null, "Admin", "ADMIN" },
                    { new Guid("fc839d07-4ece-43da-8377-4b82110d5651"), null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e"), 0, "6703a3fc-f9c2-4b5f-b131-30410e4d49cf", "admin@example.com", true, false, null, "Unknown", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEH0z24HTY/ip5gPB/sg8zs5hatXJGltfonGzCQrMjEIXMQsXm0oNDOd/nAo3+LMyRg==", null, false, "53183e33-0bf0-489a-b659-3a4a8260a968", false, "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"), new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d1ef53ac-aab9-4fa0-ba8c-c3f69505a62e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("49bda4f3-b395-4723-8e83-f448d4f7fa8e"));
        }
    }
}
