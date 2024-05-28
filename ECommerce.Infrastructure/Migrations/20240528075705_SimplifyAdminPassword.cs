using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("369b6eed-1926-4ea7-a3e0-73810ed9af19"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "73fbbf33-c9e3-4433-a902-11b1b779cb18", "AQAAAAIAAYagAAAAEKJmp3FPqpk8zzTtvMGh0TbS3zvj2+hbJ8E+geqoy13BDk51/70Swu4eDqzzV9avxg==", "87a784e9-9448-4ed5-b306-d994213a6158" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("369b6eed-1926-4ea7-a3e0-73810ed9af19"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7c1b482-c7a4-47db-ad5a-aff4059bcd96", "AQAAAAIAAYagAAAAECx7eQ6p2daZLekT6o0VtbEmV4ITcPj6fzkWCuV1wbmdWT7CCIbs30uHExCVAgonUQ==", "1995a329-7b66-406b-bc5c-3840d34b25aa" });
        }
    }
}
