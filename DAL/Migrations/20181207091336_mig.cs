using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "VendorItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "VendorItemGalleries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "VendorCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "SocialMedias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "OffersGalleries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "Blogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "VendorItems");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "VendorItemGalleries");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "VendorCategories");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "SocialMedias");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "OffersGalleries");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Blogs");
        }
    }
}
