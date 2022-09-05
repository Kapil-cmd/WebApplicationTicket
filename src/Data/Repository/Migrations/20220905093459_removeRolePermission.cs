using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class removeRolePermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "ParentPermissionId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "RolePermissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentPermissionId",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
