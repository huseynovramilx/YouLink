using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class DropNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Links_Number",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Links");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Links",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Links_Number",
                table: "Links",
                column: "Number",
                unique: true);
        }
    }
}
