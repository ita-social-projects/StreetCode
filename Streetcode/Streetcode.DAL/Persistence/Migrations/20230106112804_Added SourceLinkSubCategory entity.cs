using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddedSourceLinkSubCategoryentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_link_subcategories",
                columns: new[] { "Id", "SourceLinkCategoryId", "Title" },
                values: new object[,]
                {
                    { 1, 3, "Книги про Грушевьского" },
                    { 2, 1, "Праці Грушевьского" },
                    { 3, 1, "Фільми про Грушевьского" }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 13, 28, 3, 833, DateTimeKind.Local).AddTicks(3227));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 13, 28, 3, 833, DateTimeKind.Local).AddTicks(3149));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 13, 28, 3, 833, DateTimeKind.Local).AddTicks(3197));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 6, 13, 28, 3, 833, DateTimeKind.Local).AddTicks(3202));

            migrationBuilder.CreateIndex(
                name: "IX_source_link_subcategories_SourceLinkCategoryId",
                schema: "sources",
                table: "source_link_subcategories",
                column: "SourceLinkCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "source_link_subcategories",
                schema: "sources");

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
        }
    }
}
