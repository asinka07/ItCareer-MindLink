using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MindLink.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserCode", "Birthday", "CreatedAt", "Gender", "LastLogin", "Name", "Password", "RoleId", "Username" },
                values: new object[,]
                {
                    { "111111", new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 14, 9, 43, 19, 140, DateTimeKind.Unspecified), "f", new DateTime(2026, 2, 14, 22, 46, 55, 879, DateTimeKind.Unspecified).AddTicks(7045), "Error", "111111", 1, "Error" },
                    { "bLZoUT", new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 14, 9, 43, 19, 140, DateTimeKind.Unspecified), "m", new DateTime(2026, 2, 14, 22, 46, 55, 879, DateTimeKind.Unspecified).AddTicks(7045), "Super Admin", "AQAAAAIAAYagAAAAEJJG/NCL8BXPg/UXNCdW63SHrXqyt4M/Yuf5jkyxzlJhBUdahGYJiAJsc4ioN89azA==", 2, "admin_user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserCode",
                keyValue: "111111");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserCode",
                keyValue: "bLZoUT");
        }
    }
}
