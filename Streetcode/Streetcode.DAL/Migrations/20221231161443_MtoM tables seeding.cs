using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Migrations
{
    public partial class MtoMtablesseeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourceLinkSourceLinkCategory_source_link_categories_CategoriesId",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_SourceLinkSourceLinkCategory_source_links_SourceLinksId",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SourceLinkSourceLinkCategory",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory");

            migrationBuilder.RenameTable(
                name: "streetcode_partner",
                schema: "partners",
                newName: "streetcode_partner",
                newSchema: "streetcode");

            migrationBuilder.RenameTable(
                name: "SourceLinkSourceLinkCategory",
                schema: "sources",
                newName: "source_link_source_link_category",
                newSchema: "sources");

            migrationBuilder.RenameIndex(
                name: "IX_SourceLinkSourceLinkCategory_SourceLinksId",
                schema: "sources",
                table: "source_link_source_link_category",
                newName: "IX_source_link_source_link_category_SourceLinksId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_source_link_source_link_category",
                schema: "sources",
                table: "source_link_source_link_category",
                columns: new[] { "CategoriesId", "SourceLinksId" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "arts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                schema: "timeline",
                table: "historical_contexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Дитинство");

            migrationBuilder.UpdateData(
                schema: "timeline",
                table: "historical_contexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Студентство");

            migrationBuilder.UpdateData(
                schema: "timeline",
                table: "historical_contexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Життя в Петербурзі");

            migrationBuilder.InsertData(
                schema: "media",
                table: "images",
                columns: new[] { "Id", "Alt", "Title", "Url" },
                values: new object[,]
                {
                    { 9, "book", "book", "https://marvistamom.com/wp-content/uploads/books3.jpg" },
                    { 10, "video", "video", "https://www.earnmydegree.com/sites/all/files/public/video-prod-image.jpg" },
                    { 11, "article", "article", "https://images.laws.com/constitution/constitutional-convention.jpg" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "related_figures",
                columns: new[] { "ObserverId", "TargetId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 3 }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 31, 18, 14, 43, 121, DateTimeKind.Local).AddTicks(3542));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 31, 18, 14, 43, 121, DateTimeKind.Local).AddTicks(3326));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 31, 18, 14, 43, 121, DateTimeKind.Local).AddTicks(3374));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 31, 18, 14, 43, 121, DateTimeKind.Local).AddTicks(3377));

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageId",
                value: 11);

            migrationBuilder.AddForeignKey(
                name: "FK_source_link_source_link_category_source_link_categories_CategoriesId",
                schema: "sources",
                table: "source_link_source_link_category",
                column: "CategoriesId",
                principalSchema: "sources",
                principalTable: "source_link_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_source_link_source_link_category_source_links_SourceLinksId",
                schema: "sources",
                table: "source_link_source_link_category",
                column: "SourceLinksId",
                principalSchema: "sources",
                principalTable: "source_links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_source_link_source_link_category_source_link_categories_CategoriesId",
                schema: "sources",
                table: "source_link_source_link_category");

            migrationBuilder.DropForeignKey(
                name: "FK_source_link_source_link_category_source_links_SourceLinksId",
                schema: "sources",
                table: "source_link_source_link_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_source_link_source_link_category",
                schema: "sources",
                table: "source_link_source_link_category");

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.RenameTable(
                name: "streetcode_partner",
                schema: "streetcode",
                newName: "streetcode_partner",
                newSchema: "partners");

            migrationBuilder.RenameTable(
                name: "source_link_source_link_category",
                schema: "sources",
                newName: "SourceLinkSourceLinkCategory",
                newSchema: "sources");

            migrationBuilder.RenameIndex(
                name: "IX_source_link_source_link_category_SourceLinksId",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory",
                newName: "IX_SourceLinkSourceLinkCategory_SourceLinksId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourceLinkSourceLinkCategory",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory",
                columns: new[] { "CategoriesId", "SourceLinksId" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "arts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "«Погруддя жінки» — портрет роботи Тараса Шевченка (копія з невідомого оригіналу) виконаний ним у Вільно в 1830 році на папері італійським олівцем. Розмір 47,5 × 38. Зустрічається також під назвою «Жіноча голівка»");

            migrationBuilder.UpdateData(
                schema: "timeline",
                table: "historical_contexts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "Book");

            migrationBuilder.UpdateData(
                schema: "timeline",
                table: "historical_contexts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Wideo");

            migrationBuilder.UpdateData(
                schema: "timeline",
                table: "historical_contexts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "Article");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2742));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2566));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2686));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2693));

            migrationBuilder.AddForeignKey(
                name: "FK_SourceLinkSourceLinkCategory_source_link_categories_CategoriesId",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory",
                column: "CategoriesId",
                principalSchema: "sources",
                principalTable: "source_link_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SourceLinkSourceLinkCategory_source_links_SourceLinksId",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory",
                column: "SourceLinksId",
                principalSchema: "sources",
                principalTable: "source_links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
