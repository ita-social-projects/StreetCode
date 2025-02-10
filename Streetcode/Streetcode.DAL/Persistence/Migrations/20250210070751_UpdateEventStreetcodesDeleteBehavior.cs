using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventStreetcodesDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_event_EventId",
                table: "EventStreetcodes");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodeId",
                table: "EventStreetcodes");

            migrationBuilder.AddForeignKey(
                name: "FK_EventStreetcodes_event_EventId",
                table: "EventStreetcodes",
                column: "EventId",
                principalSchema: "events",
                principalTable: "event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodeId",
                table: "EventStreetcodes",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_event_EventId",
                table: "EventStreetcodes");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodeId",
                table: "EventStreetcodes");

            migrationBuilder.AddForeignKey(
                name: "FK_EventStreetcodes_event_EventId",
                table: "EventStreetcodes",
                column: "EventId",
                principalSchema: "events",
                principalTable: "event",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodeId",
                table: "EventStreetcodes",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id");
        }
    }
}
