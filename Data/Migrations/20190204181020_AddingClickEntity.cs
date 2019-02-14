using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class AddingClickEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Links");

            migrationBuilder.CreateTable(
                name: "Clicks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    LinkId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clicks_Links_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_LinkId",
                table: "Clicks",
                column: "LinkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clicks");

            migrationBuilder.AddColumn<int>(
                name: "Clicks",
                table: "Links",
                nullable: false,
                defaultValue: 0);
        }
    }
}
