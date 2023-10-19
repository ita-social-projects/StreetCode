using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class ArtsToStreetcodeCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StreetcodeContentId",
                schema: "media",
                table: "arts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_arts_StreetcodeContentId",
                schema: "media",
                table: "arts",
                column: "StreetcodeContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_arts_streetcodes_StreetcodeContentId",
                schema: "media",
                table: "arts",
                column: "StreetcodeContentId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arts_streetcodes_StreetcodeContentId",
                schema: "media",
                table: "arts");

            migrationBuilder.DropIndex(
                name: "IX_arts_StreetcodeContentId",
                schema: "media",
                table: "arts");

            migrationBuilder.DropColumn(
                name: "StreetcodeContentId",
                schema: "media",
                table: "arts");
        }
    }
}
