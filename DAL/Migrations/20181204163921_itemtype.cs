using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class itemtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorTypeValues_VendorTypes_VendorTypeId",
                table: "VendorTypeValues");

            migrationBuilder.DropColumn(
                name: "VendorCategory",
                table: "VendorTypes");

            migrationBuilder.AlterColumn<int>(
                name: "VendorTypeId",
                table: "VendorTypeValues",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "VendorTypes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "VendorCategoryId",
                table: "VendorTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorTypes_VendorCategoryId",
                table: "VendorTypes",
                column: "VendorCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorTypes_VendorCategories_VendorCategoryId",
                table: "VendorTypes",
                column: "VendorCategoryId",
                principalTable: "VendorCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorTypeValues_VendorTypes_VendorTypeId",
                table: "VendorTypeValues",
                column: "VendorTypeId",
                principalTable: "VendorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorTypes_VendorCategories_VendorCategoryId",
                table: "VendorTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorTypeValues_VendorTypes_VendorTypeId",
                table: "VendorTypeValues");

            migrationBuilder.DropIndex(
                name: "IX_VendorTypes_VendorCategoryId",
                table: "VendorTypes");

            migrationBuilder.DropColumn(
                name: "VendorCategoryId",
                table: "VendorTypes");

            migrationBuilder.AlterColumn<int>(
                name: "VendorTypeId",
                table: "VendorTypeValues",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "VendorTypes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCategory",
                table: "VendorTypes",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorTypeValues_VendorTypes_VendorTypeId",
                table: "VendorTypeValues",
                column: "VendorTypeId",
                principalTable: "VendorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
