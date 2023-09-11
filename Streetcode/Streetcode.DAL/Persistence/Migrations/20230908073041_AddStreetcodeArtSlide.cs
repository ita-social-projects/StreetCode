using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddStreetcodeArtSlide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_streetcodes_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.RenameColumn(
                name: "StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                newName: "StreetcodeArtSlideId");

            migrationBuilder.RenameIndex(
                name: "IX_streetcode_art_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                newName: "IX_streetcode_art_StreetcodeArtSlideId");

            migrationBuilder.RenameIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                newName: "IX_streetcode_art_ArtId_StreetcodeArtSlideId");

            migrationBuilder.CreateTable(
                name: "streetcode_art_slide",
                schema: "streetcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_art_slide", x => x.Id);
                    table.ForeignKey(
                        name: "FK_streetcode_art_slide_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_art_slide_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art_slide",
                column: "StreetcodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeArtSlideId",
                principalSchema: "streetcode",
                principalTable: "streetcode_art_slide",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropTable(
                name: "streetcode_art_slide",
                schema: "streetcode");

            migrationBuilder.RenameColumn(
                name: "StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                newName: "StreetcodeId");

            migrationBuilder.RenameIndex(
                name: "IX_streetcode_art_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                newName: "IX_streetcode_art_StreetcodeId");

            migrationBuilder.RenameIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                newName: "IX_streetcode_art_ArtId_StreetcodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_streetcodes_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
