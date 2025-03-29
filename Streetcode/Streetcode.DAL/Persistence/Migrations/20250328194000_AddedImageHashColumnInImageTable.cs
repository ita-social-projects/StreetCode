using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageHashColumnInImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_favourites_streetcodes_StreetcodeId",
            //     schema: "streetcode",
            //     table: "favourites");

            migrationBuilder.AddColumn<decimal>(
                name: "ImageHash",
                schema: "media",
                table: "images",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_favourites_streetcodes_StreetcodeId",
            //     schema: "streetcode",
            //     table: "favourites",
            //     column: "StreetcodeId",
            //     principalSchema: "streetcode",
            //     principalTable: "streetcodes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_favourites_streetcodes_StreetcodeId",
            //     schema: "streetcode",
            //     table: "favourites");

            migrationBuilder.DropColumn(
                name: "ImageHash",
                schema: "media",
                table: "images");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_favourites_streetcodes_StreetcodeId",
            //     schema: "streetcode",
            //     table: "favourites",
            //     column: "StreetcodeId",
            //     principalSchema: "streetcode",
            //     principalTable: "streetcodes",
            //     principalColumn: "Id");
        }
    }
}
