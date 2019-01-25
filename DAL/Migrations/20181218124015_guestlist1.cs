using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class guestlist1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "GuestLists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "GuestLists");
        }
    }
}
