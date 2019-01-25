using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class reviews1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VendorItemReviews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "VendorItemReviews");
        }
    }
}
