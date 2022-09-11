using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class assignedTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Users_UserId",
                table: "UserTickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserTickets",
                type: "nvarchar(25)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Users_UserId",
                table: "UserTickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTickets_Users_UserId",
                table: "UserTickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserTickets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTickets_Users_UserId",
                table: "UserTickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
