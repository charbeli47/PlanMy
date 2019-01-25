using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class itemtype1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorItemTypeValues_VendorTypes_VendorTypeId",
                table: "VendorItemTypeValues");

            migrationBuilder.DropIndex(
                name: "IX_VendorItemTypeValues_VendorTypeId",
                table: "VendorItemTypeValues");

            migrationBuilder.DropColumn(
                name: "VendorTypeId",
                table: "VendorItemTypeValues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorTypeId",
                table: "VendorItemTypeValues",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorItemTypeValues_VendorTypeId",
                table: "VendorItemTypeValues",
                column: "VendorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorItemTypeValues_VendorTypes_VendorTypeId",
                table: "VendorItemTypeValues",
                column: "VendorTypeId",
                principalTable: "VendorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
