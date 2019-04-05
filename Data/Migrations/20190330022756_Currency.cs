using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class Currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RecipientTypes_RecipientTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RecipientTypes");

            migrationBuilder.RenameColumn(
                name: "RecipientTypeID",
                table: "AspNetUsers",
                newName: "DefaultRecipientSettingsID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_RecipientTypeID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_DefaultRecipientSettingsID");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    MoneyPerImpression = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RecipientSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Receiver = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientSettings", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrencyID",
                table: "AspNetUsers",
                column: "CurrencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyID",
                table: "AspNetUsers",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RecipientSettings_DefaultRecipientSettingsID",
                table: "AspNetUsers",
                column: "DefaultRecipientSettingsID",
                principalTable: "RecipientSettings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RecipientSettings_DefaultRecipientSettingsID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "RecipientSettings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CurrencyID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "DefaultRecipientSettingsID",
                table: "AspNetUsers",
                newName: "RecipientTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_DefaultRecipientSettingsID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_RecipientTypeID");

            migrationBuilder.CreateTable(
                name: "RecipientTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Method = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientTypes", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RecipientTypes_RecipientTypeID",
                table: "AspNetUsers",
                column: "RecipientTypeID",
                principalTable: "RecipientTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
