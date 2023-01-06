using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddedSourceLinkSubCategorymodelanditsrelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "source_link_source_link_category",
                schema: "sources");

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
                column: "Title",
                value: "Книги");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Фільми");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Цитати");

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_link_subcategories",
                columns: new[] { "Id", "SourceLinkCategoryId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Том 9: Історичні студії та розвідки (1917–1923)." },
                    { 2, 1, "Том 10: Історичні студії та розвідки (1917–1923)." },
                    { 3, 3, "Том 12: Історичні студії та розвідки (1917–1923)." },
                    { 4, 1, "Том 11: Історичні студії та розвідки (1917–1923)." }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 16, 44, 0, 37, DateTimeKind.Local).AddTicks(1282));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 16, 44, 0, 37, DateTimeKind.Local).AddTicks(1198));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 16, 44, 0, 37, DateTimeKind.Local).AddTicks(1261));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 16, 44, 0, 37, DateTimeKind.Local).AddTicks(1266));

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "source_link_source_link_subcategory",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "source_link_subcategories",
                schema: "sources");

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
                name: "IX_source_link_source_link_category_SourceLinksId",
                schema: "sources",
                table: "source_link_source_link_category",
                column: "SourceLinksId");
        }
    }
}
