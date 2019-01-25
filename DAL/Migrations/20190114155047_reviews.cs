using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class reviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorItemReviews",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    comment = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: true),
                    rating = table.Column<int>(nullable: false),
                    VendorItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorItemReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorItemReviews_VendorItems_VendorItemId",
                        column: x => x.VendorItemId,
                        principalTable: "VendorItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorItemReviews_VendorItemId",
                table: "VendorItemReviews",
                column: "VendorItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorItemReviews");
        }
    }
}
