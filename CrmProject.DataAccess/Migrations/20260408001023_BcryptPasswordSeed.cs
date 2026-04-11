using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BcryptPasswordSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$9TdjMGviuzAhCZ0xgyd4peEGIAN3tlgCJl8/5Qe7Xt77pu6sS1b.S");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "password");
        }
    }
}
