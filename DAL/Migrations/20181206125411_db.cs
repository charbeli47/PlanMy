using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Pages",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "HomeTips",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "HomeTips",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "BlogCategories",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "BlogCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "BlogCategories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "HomeTips");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "HomeTips");

            migrationBuilder.DropColumn(
                name: "Media",
                table: "BlogCategories");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "BlogCategories");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Pages",
                newName: "title");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "BlogCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
