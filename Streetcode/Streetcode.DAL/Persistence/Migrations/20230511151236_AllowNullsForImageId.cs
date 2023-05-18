using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AllowNullsForImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.DropIndex(
                name: "IX_news_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "news",
                table: "news",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_news_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.DropIndex(
                name: "IX_news_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "news",
                table: "news",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
