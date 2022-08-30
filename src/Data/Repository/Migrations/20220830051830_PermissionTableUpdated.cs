using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class PermissionTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControllerName",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "ActionName",
                table: "Permissions",
                newName: "ParentPermissionId");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "ParentPermissionId",
                table: "Permissions",
                newName: "ActionName");

            migrationBuilder.AddColumn<string>(
                name: "ControllerName",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
