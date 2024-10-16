using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImageConstaintChange : Migration
    {
        /// <inheritdoc />
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

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "sources",
                table: "source_link_categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "jobs",
                table: "job",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

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
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
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

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "sources",
                table: "source_link_categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "news",
                table: "news",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "jobs",
                table: "job",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

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
    }
}
