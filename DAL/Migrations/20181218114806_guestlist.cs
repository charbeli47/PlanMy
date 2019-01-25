using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class guestlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "GuestLists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "GuestLists");
        }
    }
}
