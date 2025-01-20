using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimelineItemNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_event_timeline_items_TimelineItemId",
                schema: "events",
                table: "event");

            migrationBuilder.AddForeignKey(
                name: "FK_event_timeline_items_TimelineItemId",
                schema: "events",
                table: "event",
                column: "TimelineItemId",
                principalSchema: "timeline",
                principalTable: "timeline_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_event_timeline_items_TimelineItemId",
                schema: "events",
                table: "event");

            migrationBuilder.AddForeignKey(
                name: "FK_event_timeline_items_TimelineItemId",
                schema: "events",
                table: "event",
                column: "TimelineItemId",
                principalSchema: "timeline",
                principalTable: "timeline_items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
