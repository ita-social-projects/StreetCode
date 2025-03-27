using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "events");

            migrationBuilder.CreateTable(
                name: "event",
                schema: "events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 500, nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Organizer = table.Column<string>(type: "nvarchar(100)", maxLength: 200, nullable: true),
                    DateString = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TimelineItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_event_timeline_items_TimelineItemId",
                        column: x => x.TimelineItemId,
                        principalSchema: "timeline",
                        principalTable: "timeline_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EventStreetcodes",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStreetcodes", x => new { x.EventId, x.StreetcodeId });
                    table.ForeignKey(
                        name: "FK_EventStreetcodes_event_EventId",
                        column: x => x.EventId,
                        principalSchema: "events",
                        principalTable: "event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventStreetcodes_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_TimelineItemId",
                schema: "events",
                table: "event",
                column: "TimelineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EventStreetcodes_StreetcodeId",
                table: "EventStreetcodes",
                column: "StreetcodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.DropTable(
                name: "EventStreetcodes");

            migrationBuilder.DropTable(
                name: "event",
                schema: "events");

            migrationBuilder.AddForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
