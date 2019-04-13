using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShortener.Data.Migrations
{
    public partial class RequiredCurrency1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RecipientSettings_DefaultRecipientSettingsID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PayoutRequests_AspNetUsers_OwnerId",
                table: "PayoutRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientSettings_RecipientType_RecipientTypeID",
                table: "RecipientSettings");

            migrationBuilder.DropIndex(
                name: "IX_PayoutRequests_OwnerId",
                table: "PayoutRequests");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DefaultRecipientSettingsID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientType",
                table: "RecipientType");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "PayoutRequests");

            migrationBuilder.DropColumn(
                name: "DefaultRecipientSettingsID",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "RecipientType",
                newName: "RecipientTypes");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "RecipientSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipientSettingsID",
                table: "PayoutRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientTypes",
                table: "RecipientTypes",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipientSettings_OwnerId",
                table: "RecipientSettings",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PayoutRequests_RecipientSettingsID",
                table: "PayoutRequests",
                column: "RecipientSettingsID");

            migrationBuilder.AddForeignKey(
                name: "FK_PayoutRequests_RecipientSettings_RecipientSettingsID",
                table: "PayoutRequests",
                column: "RecipientSettingsID",
                principalTable: "RecipientSettings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientSettings_AspNetUsers_OwnerId",
                table: "RecipientSettings",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientSettings_RecipientTypes_RecipientTypeID",
                table: "RecipientSettings",
                column: "RecipientTypeID",
                principalTable: "RecipientTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayoutRequests_RecipientSettings_RecipientSettingsID",
                table: "PayoutRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientSettings_AspNetUsers_OwnerId",
                table: "RecipientSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipientSettings_RecipientTypes_RecipientTypeID",
                table: "RecipientSettings");

            migrationBuilder.DropIndex(
                name: "IX_RecipientSettings_OwnerId",
                table: "RecipientSettings");

            migrationBuilder.DropIndex(
                name: "IX_PayoutRequests_RecipientSettingsID",
                table: "PayoutRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipientTypes",
                table: "RecipientTypes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "RecipientSettings");

            migrationBuilder.DropColumn(
                name: "RecipientSettingsID",
                table: "PayoutRequests");

            migrationBuilder.RenameTable(
                name: "RecipientTypes",
                newName: "RecipientType");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "PayoutRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultRecipientSettingsID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipientType",
                table: "RecipientType",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_PayoutRequests_OwnerId",
                table: "PayoutRequests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DefaultRecipientSettingsID",
                table: "AspNetUsers",
                column: "DefaultRecipientSettingsID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RecipientSettings_DefaultRecipientSettingsID",
                table: "AspNetUsers",
                column: "DefaultRecipientSettingsID",
                principalTable: "RecipientSettings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayoutRequests_AspNetUsers_OwnerId",
                table: "PayoutRequests",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipientSettings_RecipientType_RecipientTypeID",
                table: "RecipientSettings",
                column: "RecipientTypeID",
                principalTable: "RecipientType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
