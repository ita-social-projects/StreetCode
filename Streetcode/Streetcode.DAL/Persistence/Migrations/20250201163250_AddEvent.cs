using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_event_EventsId",
                table: "EventStreetcodes");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodesId",
                table: "EventStreetcodes");

            migrationBuilder.RenameColumn(
                name: "StreetcodesId",
                table: "EventStreetcodes",
                newName: "StreetcodeId");

            migrationBuilder.RenameColumn(
                name: "EventsId",
                table: "EventStreetcodes",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventStreetcodes_StreetcodesId",
                table: "EventStreetcodes",
                newName: "IX_EventStreetcodes_StreetcodeId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_event_EventId",
                table: "EventStreetcodes");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodeId",
                table: "EventStreetcodes");

            migrationBuilder.RenameColumn(
                name: "StreetcodeId",
                table: "EventStreetcodes",
                newName: "StreetcodesId");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventStreetcodes",
                newName: "EventsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventStreetcodes_StreetcodeId",
                table: "EventStreetcodes",
                newName: "IX_EventStreetcodes_StreetcodesId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventStreetcodes_event_EventsId",
                table: "EventStreetcodes",
                column: "EventsId",
                principalSchema: "events",
                principalTable: "event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStreetcodes_streetcodes_StreetcodesId",
                table: "EventStreetcodes",
                column: "StreetcodesId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
