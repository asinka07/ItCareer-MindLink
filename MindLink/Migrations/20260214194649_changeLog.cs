using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindLink.Migrations
{
    /// <inheritdoc />
    public partial class changeLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    UserCode = table.Column<string>(type: "char(6)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.UserCode);
                    table.ForeignKey(
                        name: "FK_Log_Users_UserCode",
                        column: x => x.UserCode,
                        principalTable: "Users",
                        principalColumn: "UserCode",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");
        }
    }
}
