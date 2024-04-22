using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class RefactorStreetcodeArts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_arts_ArtId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_streetcodes_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.AlterColumn<int>(
                name: "StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "streetcode",
                table: "streetcode_art",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeId",
                schema: "media",
                table: "arts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art",
                column: "Id");

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
                name: "IX_arts_StreetcodeId",
                schema: "media",
                table: "arts",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_art_slide_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art_slide",
                column: "StreetcodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_arts_streetcodes_StreetcodeId",
                schema: "media",
                table: "arts",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_arts_ArtId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "ArtId",
                principalSchema: "media",
                principalTable: "arts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeArtSlideId",
                principalSchema: "streetcode",
                principalTable: "streetcode_art_slide",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_streetcodes_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arts_streetcodes_StreetcodeId",
                schema: "media",
                table: "arts");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_arts_ArtId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_art_streetcodes_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropTable(
                name: "streetcode_art_slide",
                schema: "streetcode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_streetcode_art",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_art_ArtId_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_streetcode_art_StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropIndex(
                name: "IX_arts_StreetcodeId",
                schema: "media",
                table: "arts");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropColumn(
                name: "StreetcodeArtSlideId",
                schema: "streetcode",
                table: "streetcode_art");

            migrationBuilder.DropColumn(
                name: "StreetcodeId",
                schema: "media",
                table: "arts");

            migrationBuilder.AlterColumn<int>(
                name: "StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_art_arts_ArtId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "ArtId",
                principalSchema: "media",
                principalTable: "arts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
