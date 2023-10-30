using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddStreetcodeArtSlides : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "streetcode",
                table: "streetcode_art",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art",
                column: "Id");

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeContentId",
                schema: "media",
                table: "arts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "streetcode_art_slide",
                schema: "streetcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false),
                    Template = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_streetcode_art_ArtId_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeArtSlideId" });

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_art_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeArtSlideId");

            migrationBuilder.CreateIndex(
                name: "IX_arts_StreetcodeContentId",
                schema: "media",
                table: "arts",
                column: "StreetcodeContentId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_art_slide_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art_slide",
                column: "StreetcodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_arts_streetcodes_StreetcodeContentId",
                schema: "media",
                table: "arts",
                column: "StreetcodeContentId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeArtSlideId",
                principalSchema: "streetcode",
                principalTable: "streetcode_art_slide",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropForeignKey(
                name: "FK_arts_streetcodes_StreetcodeContentId",
                schema: "media",
                table: "arts");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropTable(
                name: "streetcode_art_slide",
                schema: "streetcode");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_art_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_arts_StreetcodeContentId",
                schema: "media",
                table: "arts");

            migrationBuilder.DropColumn(
                name: "StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropColumn(
                name: "StreetcodeContentId",
                schema: "media",
                table: "arts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeId" });

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeId" });
        }
    }
}
