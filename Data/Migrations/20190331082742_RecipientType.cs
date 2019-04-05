using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class RecipientType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RecipientSettings");

            migrationBuilder.AddColumn<int>(
                name: "RecipientTypeID",
                table: "RecipientSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecipientType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientType", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipientSettings_RecipientTypeID",
                table: "RecipientSettings",
                column: "RecipientTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientSettings_RecipientType_RecipientTypeID",
                table: "RecipientSettings",
                column: "RecipientTypeID",
                principalTable: "RecipientType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipientSettings_RecipientType_RecipientTypeID",
                table: "RecipientSettings");

            migrationBuilder.DropTable(
                name: "RecipientType");

            migrationBuilder.DropIndex(
                name: "IX_RecipientSettings_RecipientTypeID",
                table: "RecipientSettings");

            migrationBuilder.DropColumn(
                name: "RecipientTypeID",
                table: "RecipientSettings");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecipientSettings",
                nullable: false,
                defaultValue: "");
        }
    }
}
