using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindLink.Migrations
{
    /// <inheritdoc />
    public partial class DbChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Log",
                table: "Log");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Log",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log",
                table: "Log",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserCode",
                table: "Log",
                column: "UserCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Log",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_UserCode",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Log");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log",
                table: "Log",
                column: "UserCode");
        }
    }
}
