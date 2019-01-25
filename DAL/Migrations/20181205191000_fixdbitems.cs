using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fixdbitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorItemTypeValues_VendorTypeValues_VendorTypeValueId",
                table: "VendorItemTypeValues");

            migrationBuilder.AlterColumn<int>(
                name: "VendorTypeValueId",
                table: "VendorItemTypeValues",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorItemTypeValues_VendorTypeValues_VendorTypeValueId",
                table: "VendorItemTypeValues",
                column: "VendorTypeValueId",
                principalTable: "VendorTypeValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorItemTypeValues_VendorTypeValues_VendorTypeValueId",
                table: "VendorItemTypeValues");

            migrationBuilder.AlterColumn<int>(
                name: "VendorTypeValueId",
                table: "VendorItemTypeValues",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VendorItemTypeValues_VendorTypeValues_VendorTypeValueId",
                table: "VendorItemTypeValues",
                column: "VendorTypeValueId",
                principalTable: "VendorTypeValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
