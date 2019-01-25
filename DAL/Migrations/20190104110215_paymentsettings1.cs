using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class paymentsettings1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPaymentToken_PaymentSetting_PaymentSettingId",
                table: "UserPaymentToken");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPaymentToken_AspNetUsers_UserId",
                table: "UserPaymentToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPaymentToken",
                table: "UserPaymentToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentSetting",
                table: "PaymentSetting");

            migrationBuilder.RenameTable(
                name: "UserPaymentToken",
                newName: "UserPaymentTokens");

            migrationBuilder.RenameTable(
                name: "PaymentSetting",
                newName: "PaymentSettings");

            migrationBuilder.RenameIndex(
                name: "IX_UserPaymentToken_UserId",
                table: "UserPaymentTokens",
                newName: "IX_UserPaymentTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPaymentToken_PaymentSettingId",
                table: "UserPaymentTokens",
                newName: "IX_UserPaymentTokens_PaymentSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPaymentTokens",
                table: "UserPaymentTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentSettings",
                table: "PaymentSettings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPaymentTokens_PaymentSettings_PaymentSettingId",
                table: "UserPaymentTokens",
                column: "PaymentSettingId",
                principalTable: "PaymentSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPaymentTokens_AspNetUsers_UserId",
                table: "UserPaymentTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPaymentTokens_PaymentSettings_PaymentSettingId",
                table: "UserPaymentTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPaymentTokens_AspNetUsers_UserId",
                table: "UserPaymentTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPaymentTokens",
                table: "UserPaymentTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentSettings",
                table: "PaymentSettings");

            migrationBuilder.RenameTable(
                name: "UserPaymentTokens",
                newName: "UserPaymentToken");

            migrationBuilder.RenameTable(
                name: "PaymentSettings",
                newName: "PaymentSetting");

            migrationBuilder.RenameIndex(
                name: "IX_UserPaymentTokens_UserId",
                table: "UserPaymentToken",
                newName: "IX_UserPaymentToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPaymentTokens_PaymentSettingId",
                table: "UserPaymentToken",
                newName: "IX_UserPaymentToken_PaymentSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPaymentToken",
                table: "UserPaymentToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentSetting",
                table: "PaymentSetting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPaymentToken_PaymentSetting_PaymentSettingId",
                table: "UserPaymentToken",
                column: "PaymentSettingId",
                principalTable: "PaymentSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPaymentToken_AspNetUsers_UserId",
                table: "UserPaymentToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
