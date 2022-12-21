using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class UpdateStreatcodeTimelineItemEntityConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_timeline_item_streetcodes_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_timeline_item");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_timeline_item_timeline_items_TimelineItemsId",
                schema: "streetcode",
                table: "streetcode_timeline_item");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_timeline_item_TimelineItemsId",
                schema: "streetcode",
                table: "streetcode_timeline_item");

            migrationBuilder.CreateTable(
                name: "StreetcodeContentTimelineItem",
                columns: table => new
                {
                    StreetcodesId = table.Column<int>(type: "int", nullable: false),
                    TimelineItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetcodeContentTimelineItem", x => new { x.StreetcodesId, x.TimelineItemsId });
                    table.ForeignKey(
                        name: "FK_StreetcodeContentTimelineItem_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreetcodeContentTimelineItem_timeline_items_TimelineItemsId",
                        column: x => x.TimelineItemsId,
                        principalSchema: "timeline",
                        principalTable: "timeline_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 42, 51, 496, DateTimeKind.Local).AddTicks(1000));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 42, 51, 496, DateTimeKind.Local).AddTicks(935));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 42, 51, 496, DateTimeKind.Local).AddTicks(980));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 42, 51, 496, DateTimeKind.Local).AddTicks(984));

            migrationBuilder.CreateIndex(
                name: "IX_StreetcodeContentTimelineItem_TimelineItemsId",
                table: "StreetcodeContentTimelineItem",
                column: "TimelineItemsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreetcodeContentTimelineItem");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 39, 57, 929, DateTimeKind.Local).AddTicks(7374));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 39, 57, 929, DateTimeKind.Local).AddTicks(7314));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 39, 57, 929, DateTimeKind.Local).AddTicks(7350));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 21, 20, 39, 57, 929, DateTimeKind.Local).AddTicks(7355));

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_timeline_item_TimelineItemsId",
                schema: "streetcode",
                table: "streetcode_timeline_item",
                column: "TimelineItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_timeline_item_streetcodes_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_timeline_item",
                column: "StreetcodesId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_timeline_item_timeline_items_TimelineItemsId",
                schema: "streetcode",
                table: "streetcode_timeline_item",
                column: "TimelineItemsId",
                principalSchema: "timeline",
                principalTable: "timeline_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
