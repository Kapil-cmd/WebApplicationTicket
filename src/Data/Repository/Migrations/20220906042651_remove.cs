using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTicket");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryTicket",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTicket", x => new { x.CategoryId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_CategoryTicket_Categories_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Categories",
                        principalColumn: "CId");
                    table.ForeignKey(
                        name: "FK_CategoryTicket_Tickets_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTicket_TicketId",
                table: "CategoryTicket",
                column: "TicketId");
        }
    }
}
