using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$3DXZPBQJT35SYomkVoXwZO6u4EdT.dDSHYLdpLFLApTnij55ioHOa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$3DXZPBQJT35SYomkVoXwZO6u4EdT.dDSHYLdpLFLApTnij55ioHOa");
        }
    }
}
