using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AudioConstraintChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcodes_audios_AudioId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcodes_audios_AudioId",
                schema: "streetcode",
                table: "streetcodes",
                column: "AudioId",
                principalSchema: "media",
                principalTable: "audios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcodes_audios_AudioId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcodes_audios_AudioId",
                schema: "streetcode",
                table: "streetcodes",
                column: "AudioId",
                principalSchema: "media",
                principalTable: "audios",
                principalColumn: "Id");
        }
    }
}
