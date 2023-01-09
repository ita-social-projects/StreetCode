using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Migrations
{
    public partial class ChangedSourceCategoryStretcoderelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_source_link_categories_streetcodes_StreetcodeId",
                schema: "sources",
                table: "source_link_categories");

            migrationBuilder.DropIndex(
                name: "IX_source_link_categories_StreetcodeId",
                schema: "sources",
                table: "source_link_categories");

            migrationBuilder.DropColumn(
                name: "StreetcodeId",
                schema: "sources",
                table: "source_link_categories");

            migrationBuilder.CreateTable(
                name: "streetcode_source_link_categories",
                schema: "sources",
                columns: table => new
                {
                    SourceLinkCategoriesId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_source_link_categories", x => new { x.SourceLinkCategoriesId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoriesId",
                        column: x => x.SourceLinkCategoriesId,
                        principalSchema: "sources",
                        principalTable: "source_link_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_source_link_categories_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 9,
                column: "Url",
                value: "https://images.unsplash.com/photo-1589998059171-988d887df646?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8b3BlbiUyMGJvb2t8ZW58MHx8MHx8&w=1000&q=80");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(7033));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(6980));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(7016));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(7019));

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_source_link_categories_StreetcodesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                column: "StreetcodesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "streetcode_source_link_categories",
                schema: "sources");

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeId",
                schema: "sources",
                table: "source_link_categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 9,
                column: "Url",
                value: "https://marvistamom.com/wp-content/uploads/books3.jpg");

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "StreetcodeId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "StreetcodeId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "sources",
                table: "source_link_categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "StreetcodeId",
                value: 1);

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
    }
}
