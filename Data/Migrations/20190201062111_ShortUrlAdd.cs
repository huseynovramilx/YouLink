using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class ShortUrlAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Clicks",
                table: "Links",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Links",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Links_Number",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Links");

            migrationBuilder.AlterColumn<int>(
                name: "Clicks",
                table: "Links",
                nullable: true,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Links",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
