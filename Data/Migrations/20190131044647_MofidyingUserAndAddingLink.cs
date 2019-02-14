using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class MofidyingUserAndAddingLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 5, nullable: false),
                    Clicks = table.Column<int>(nullable: false),
                    FullUrl = table.Column<string>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_OwnerId",
                table: "Links",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
