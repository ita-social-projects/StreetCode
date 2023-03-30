using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddTransliterationUrlToStreetcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransliterationUrl",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                schema: "streetcode",
                table: "related_terms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "related_terms",
                columns: new[] { "Id", "TermId", "Word" },
                values: new object[] { 1, 3, "кріпаків" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "svilnennia-chersonu" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "taras-shevchenko" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "mykola-kostomarov" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "vasyl-biloservky" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "volodymir-varfolomiy-kropyvnitsky-shevchenkivkyski" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "lesya-ukrainka" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "TransliterationUrl" },
                values: new object[] { "ivan-mazepa" });

            migrationBuilder.CreateIndex(
                name: "IX_streetcodes_TransliterationUrl",
                schema: "streetcode",
                table: "streetcodes",
                column: "TransliterationUrl",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_streetcodes_TransliterationUrl",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_terms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "TransliterationUrl",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                schema: "streetcode",
                table: "related_terms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
