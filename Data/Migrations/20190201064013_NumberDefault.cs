using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class NumberDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Links",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Links_Number",
                table: "Links",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Links_Number",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Links");
        }
    }
}
