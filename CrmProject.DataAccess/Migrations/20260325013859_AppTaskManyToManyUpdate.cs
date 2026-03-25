using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AppTaskManyToManyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTasks_Users_AssignedToUserId",
                table: "AppTasks");

            migrationBuilder.DropIndex(
                name: "IX_AppTasks_AssignedToUserId",
                table: "AppTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "AppTasks");

            migrationBuilder.CreateTable(
                name: "AppTaskAssignedUsers",
                columns: table => new
                {
                    AssignedTasksId = table.Column<int>(type: "int", nullable: false),
                    AssignedUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTaskAssignedUsers", x => new { x.AssignedTasksId, x.AssignedUsersId });
                    table.ForeignKey(
                        name: "FK_AppTaskAssignedUsers_AppTasks_AssignedTasksId",
                        column: x => x.AssignedTasksId,
                        principalTable: "AppTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppTaskAssignedUsers_Users_AssignedUsersId",
                        column: x => x.AssignedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppTaskAssignedUsers_AssignedUsersId",
                table: "AppTaskAssignedUsers",
                column: "AssignedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTaskAssignedUsers");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "AppTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppTasks_AssignedToUserId",
                table: "AppTasks",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTasks_Users_AssignedToUserId",
                table: "AppTasks",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
