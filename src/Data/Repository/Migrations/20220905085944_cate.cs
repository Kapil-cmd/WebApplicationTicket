using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class cate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CategoryTicket",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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

        }

       
    }
}
