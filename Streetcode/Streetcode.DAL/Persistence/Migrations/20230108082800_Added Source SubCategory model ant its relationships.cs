using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddedSourceSubCategorymodelantitsrelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_source_links_streetcodes_StreetcodeId",
                schema: "sources",
                table: "source_links");

            migrationBuilder.DropTable(
                name: "source_link_source_link_category",
                schema: "sources");

            migrationBuilder.DropIndex(
                name: "IX_source_links_StreetcodeId",
                schema: "sources",
                table: "source_links");

            migrationBuilder.DropColumn(
                name: "StreetcodeId",
                schema: "sources",
                table: "source_links");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "sources",
                table: "source_links",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeId",
                schema: "sources",
                table: "source_link_categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "source_link_subcategories",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceLinkCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_source_link_subcategories_source_link_categories_SourceLinkCategoryId",
                        column: x => x.SourceLinkCategoryId,
                        principalSchema: "sources",
                        principalTable: "source_link_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "source_link_source_link_subcategory",
                schema: "sources",
                columns: table => new
                {
                    SourceLinksId = table.Column<int>(type: "int", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_source_link_subcategory", x => new { x.SourceLinksId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_source_link_source_link_subcategory_source_link_subcategories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalSchema: "sources",
                        principalTable: "source_link_subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_source_link_source_link_subcategory_source_links_SourceLinksId",
                        column: x => x.SourceLinksId,
                        principalSchema: "sources",
                        principalTable: "source_links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StreetcodeId", "Title" },
                values: new object[] { 1, "Книги" });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StreetcodeId", "Title" },
                values: new object[] { 1, "Фільми" });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StreetcodeId", "Title" },
                values: new object[] { 1, "Цитати" });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_link_subcategories",
                columns: new[] { "Id", "SourceLinkCategoryId", "Title" },
                values: new object[,]
                {
                    { 1, 2, "Фільми про Т. Г. Шевченко" },
                    { 2, 2, "Хроніки про Т. Г. Шевченко" },
                    { 3, 2, "Блоги про Т. Г. Шевченко" },
                    { 4, 1, "Праці Грушевського" },
                    { 5, 1, "Книги про Грушевського" },
                    { 6, 1, "Статті" },
                    { 7, 3, "Пряма мова" }
                });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Том 2: Суспільно-політичні твори (1907–1914).");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Том 3: Суспільно-політичні твори (1907 — березень 1917).");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Том 4. Книга 1: Суспільно-політичні твори (доба Української Центральної Ради, березень 1917 — квітень 1918).");

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_links",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[,]
                {
                    { 4, "Том 5: Історичні студії та розвідки (1888–1896).", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 5, "Том 6: Історичні студії та розвідки (1895–1900).", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 6, "Том 7: Історичні студії та розвідки (1900–1906).", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 7, "Том 8: Історичні студії та розвідки (1906–1916).", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 8, "Том 9: Історичні студії та розвідки (1917–1923).", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 9, "Том 10. Книга 1: Історичні студії та розвідки (1924— 1930)/ упор. О.Юркова.", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 10, "Том 10. Книга 2: Історичні студії та розвідки (1930— 1934)", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 11, "Том 11: Літературно-критичні праці (1883–1931), «По світу»", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 12, "Том 12: Поезія (1882–1903). Проза, драматичні твори, переклади (1883–1886)", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 13, "Том 13 : Серія \"Літературно-критичні та художні твори (1887-1924)\"", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 14, "Том 14: Рецензії та огляди (1888–1897).", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 8, 10, 27, 59, 889, DateTimeKind.Local).AddTicks(3654));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 8, 10, 27, 59, 889, DateTimeKind.Local).AddTicks(3597));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 8, 10, 27, 59, 889, DateTimeKind.Local).AddTicks(3636));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 8, 10, 27, 59, 889, DateTimeKind.Local).AddTicks(3640));

            migrationBuilder.CreateIndex(
                name: "IX_source_link_categories_StreetcodeId",
                schema: "sources",
                table: "source_link_categories",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_source_link_source_link_subcategory_SubCategoriesId",
                schema: "sources",
                table: "source_link_source_link_subcategory",
                column: "SubCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_source_link_subcategories_SourceLinkCategoryId",
                schema: "sources",
                table: "source_link_subcategories",
                column: "SourceLinkCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_source_link_categories_streetcodes_StreetcodeId",
                schema: "sources",
                table: "source_link_categories",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_source_link_categories_streetcodes_StreetcodeId",
                schema: "sources",
                table: "source_link_categories");

            migrationBuilder.DropTable(
                name: "source_link_source_link_subcategory",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "source_link_subcategories",
                schema: "sources");

            migrationBuilder.DropIndex(
                name: "IX_source_link_categories_StreetcodeId",
                schema: "sources",
                table: "source_link_categories");

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DropColumn(
                name: "StreetcodeId",
                schema: "sources",
                table: "source_link_categories");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "sources",
                table: "source_links",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeId",
                schema: "sources",
                table: "source_links",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "source_link_source_link_category",
                schema: "sources",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    SourceLinksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_source_link_category", x => new { x.CategoriesId, x.SourceLinksId });
                    table.ForeignKey(
                        name: "FK_source_link_source_link_category_source_link_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "sources",
                        principalTable: "source_link_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_source_link_source_link_category_source_links_SourceLinksId",
                        column: x => x.SourceLinksId,
                        principalSchema: "sources",
                        principalTable: "source_links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "book");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "video");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "article");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "StreetcodeId", "Title" },
                values: new object[] { 1, "Вікіпедія" });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "StreetcodeId", "Title" },
                values: new object[] { 1, "Кобзар" });

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "StreetcodeId", "Title" },
                values: new object[] { 4, "Св'яткування звільнення" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 4, 22, 11, 57, 551, DateTimeKind.Local).AddTicks(7284));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 4, 22, 11, 57, 551, DateTimeKind.Local).AddTicks(7208));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 4, 22, 11, 57, 551, DateTimeKind.Local).AddTicks(7256));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 4, 22, 11, 57, 551, DateTimeKind.Local).AddTicks(7259));

            migrationBuilder.CreateIndex(
                name: "IX_source_links_StreetcodeId",
                schema: "sources",
                table: "source_links",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_source_link_source_link_category_SourceLinksId",
                schema: "sources",
                table: "source_link_source_link_category",
                column: "SourceLinksId");

            migrationBuilder.AddForeignKey(
                name: "FK_source_links_streetcodes_StreetcodeId",
                schema: "sources",
                table: "source_links",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
