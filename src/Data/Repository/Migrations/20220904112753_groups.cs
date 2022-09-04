using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class groups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Categories_CategoryName",
                table: "Tickets");

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

            migrationBuilder.AlterColumn<string>(
                name: "PermissionName",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Categories_CategoryName",
                table: "Tickets",
                column: "CategoryName",
                principalTable: "Categories",
                principalColumn: "CategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Categories_CategoryName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "ParentPermissionId",
                table: "RolePermissions");

            migrationBuilder.AlterColumn<string>(
                name: "PermissionName",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Categories_CategoryName",
                table: "Tickets",
                column: "CategoryName",
                principalTable: "Categories",
                principalColumn: "CategoryName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
