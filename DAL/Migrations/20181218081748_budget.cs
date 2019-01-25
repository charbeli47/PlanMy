using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class budget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetCategories_BudgetCategoryId",
                table: "Budgets");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidCost",
                table: "Budgets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedCost",
                table: "Budgets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BudgetCategoryId",
                table: "Budgets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualCost",
                table: "Budgets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetCategories_BudgetCategoryId",
                table: "Budgets",
                column: "BudgetCategoryId",
                principalTable: "BudgetCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetCategories_BudgetCategoryId",
                table: "Budgets");

            migrationBuilder.AlterColumn<string>(
                name: "PaidCost",
                table: "Budgets",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "EstimatedCost",
                table: "Budgets",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "BudgetCategoryId",
                table: "Budgets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "ActualCost",
                table: "Budgets",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetCategories_BudgetCategoryId",
                table: "Budgets",
                column: "BudgetCategoryId",
                principalTable: "BudgetCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
