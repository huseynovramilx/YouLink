using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class LinkClicksNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_OwnerId",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Links",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Clicks",
                table: "Links",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_OwnerId",
                table: "Links",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_OwnerId",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Links",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Clicks",
                table: "Links",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_OwnerId",
                table: "Links",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
