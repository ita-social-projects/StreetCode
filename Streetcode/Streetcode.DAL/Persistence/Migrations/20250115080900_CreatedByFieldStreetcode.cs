using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatedByFieldStreetcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "streetcode",
                table: "streetcodes");

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
