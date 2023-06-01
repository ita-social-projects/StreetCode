using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class ManyToManyTimeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timeline_item_historical_context_historical_contexts_HistoricalContextId",
                schema: "timeline",
                table: "timeline_item_historical_context");

            migrationBuilder.DropForeignKey(
                name: "FK_timeline_item_historical_context_timeline_items_TimelineId",
                schema: "timeline",
                table: "timeline_item_historical_context");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timeline_item_historical_context",
                schema: "timeline",
                table: "timeline_item_historical_context");

            migrationBuilder.DropIndex(
                name: "IX_timeline_item_historical_context_TimelineId",
                schema: "timeline",
                table: "timeline_item_historical_context");

            migrationBuilder.RenameTable(
                name: "timeline_item_historical_context",
                schema: "timeline",
                newName: "HistoricalContextTimelines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricalContextTimelines",
                table: "HistoricalContextTimelines",
                columns: new[] { "TimelineId", "HistoricalContextId" });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricalContextTimelines_HistoricalContextId",
                table: "HistoricalContextTimelines",
                column: "HistoricalContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricalContextTimelines_historical_contexts_HistoricalContextId",
                table: "HistoricalContextTimelines",
                column: "HistoricalContextId",
                principalSchema: "timeline",
                principalTable: "historical_contexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricalContextTimelines_timeline_items_TimelineId",
                table: "HistoricalContextTimelines",
                column: "TimelineId",
                principalSchema: "timeline",
                principalTable: "timeline_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalContextTimelines_historical_contexts_HistoricalContextId",
                table: "HistoricalContextTimelines");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricalContextTimelines_timeline_items_TimelineId",
                table: "HistoricalContextTimelines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricalContextTimelines",
                table: "HistoricalContextTimelines");

            migrationBuilder.DropIndex(
                name: "IX_HistoricalContextTimelines_HistoricalContextId",
                table: "HistoricalContextTimelines");

            migrationBuilder.RenameTable(
                name: "HistoricalContextTimelines",
                newName: "timeline_item_historical_context",
                newSchema: "timeline");

            migrationBuilder.AddPrimaryKey(
                name: "PK_timeline_item_historical_context",
                schema: "timeline",
                table: "timeline_item_historical_context",
                columns: new[] { "HistoricalContextId", "TimelineId" });

            migrationBuilder.CreateIndex(
                name: "IX_timeline_item_historical_context_TimelineId",
                schema: "timeline",
                table: "timeline_item_historical_context",
                column: "TimelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_timeline_item_historical_context_historical_contexts_HistoricalContextId",
                schema: "timeline",
                table: "timeline_item_historical_context",
                column: "HistoricalContextId",
                principalSchema: "timeline",
                principalTable: "historical_contexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_timeline_item_historical_context_timeline_items_TimelineId",
                schema: "timeline",
                table: "timeline_item_historical_context",
                column: "TimelineId",
                principalSchema: "timeline",
                principalTable: "timeline_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
