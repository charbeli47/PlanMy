using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class paymentsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfileId = table.Column<string>(nullable: true),
                    AccessKey = table.Column<string>(nullable: true),
                    SecuritySign = table.Column<string>(nullable: true),
                    PaymentType = table.Column<int>(nullable: false),
                    RecurringFrequency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPaymentToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentSettingId = table.Column<int>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    TokenStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPaymentToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPaymentToken_PaymentSetting_PaymentSettingId",
                        column: x => x.PaymentSettingId,
                        principalTable: "PaymentSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPaymentToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentToken_PaymentSettingId",
                table: "UserPaymentToken",
                column: "PaymentSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentToken_UserId",
                table: "UserPaymentToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPaymentToken");

            migrationBuilder.DropTable(
                name: "PaymentSetting");
        }
    }
}
