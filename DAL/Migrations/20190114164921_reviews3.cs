using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class reviews3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "author",
                table: "VendorItemReviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "author",
                table: "VendorItemReviews");
        }
    }
}
