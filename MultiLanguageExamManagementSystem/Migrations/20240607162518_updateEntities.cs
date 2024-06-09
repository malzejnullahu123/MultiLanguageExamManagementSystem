using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiLanguageExamManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_LanguageId",
                table: "LocalizationResources");

            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_Namespace_Key",
                table: "LocalizationResources");

            migrationBuilder.DropIndex(
                name: "IX_Languages_CountryId",
                table: "Languages");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Languages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Languages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "Languages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_LanguageId_Namespace_Key",
                table: "LocalizationResources",
                columns: new[] { "LanguageId", "Namespace", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CountryId_LanguageCode",
                table: "Languages",
                columns: new[] { "CountryId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_LanguageId_Namespace_Key",
                table: "LocalizationResources");

            migrationBuilder.DropIndex(
                name: "IX_Languages_CountryId_LanguageCode",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_LanguageId",
                table: "LocalizationResources",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_Namespace_Key",
                table: "LocalizationResources",
                columns: new[] { "Namespace", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CountryId",
                table: "Languages",
                column: "CountryId");
        }
    }
}
