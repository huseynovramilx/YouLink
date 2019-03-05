using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class PayoutRequestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipientTypeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayoutRequests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Money = table.Column<decimal>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    Paid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayoutRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PayoutRequests_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipientTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientTypes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RecipientTypeID",
                table: "AspNetUsers",
                column: "RecipientTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PayoutRequests_OwnerId",
                table: "PayoutRequests",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RecipientTypes_RecipientTypeID",
                table: "AspNetUsers",
                column: "RecipientTypeID",
                principalTable: "RecipientTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RecipientTypes_RecipientTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PayoutRequests");

            migrationBuilder.DropTable(
                name: "RecipientTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RecipientTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RecipientTypeID",
                table: "AspNetUsers");
        }
    }
}
