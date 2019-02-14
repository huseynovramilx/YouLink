using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class StoringPayouts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "PayoutBatches",
                columns: table => new
                {
                    ID = table.Column<DateTime>(nullable: false),
                    PayoutPaypalBatchId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    EmailMessage = table.Column<string>(nullable: false),
                    EmailSubject = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayoutBatches", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Payouts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecipientType = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Receiver = table.Column<string>(nullable: false),
                    PayoutBatchId = table.Column<DateTime>(nullable: false),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payouts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payouts_PayoutBatches_PayoutBatchId",
                        column: x => x.PayoutBatchId,
                        principalTable: "PayoutBatches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_PayoutBatchId",
                table: "Payouts",
                column: "PayoutBatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payouts");

            migrationBuilder.DropTable(
                name: "PayoutBatches");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
