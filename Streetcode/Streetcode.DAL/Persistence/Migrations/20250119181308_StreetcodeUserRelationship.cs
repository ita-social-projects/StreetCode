using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StreetcodeUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_streetcodes_UserId",
                schema: "streetcode",
                table: "streetcodes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news",
                column: "ImageId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcodes_AspNetUsers_UserId",
                schema: "streetcode",
                table: "streetcodes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");


            migrationBuilder.Sql(@"
                DECLARE @FirstUserId NVARCHAR(450);
                SELECT TOP 1 @FirstUserId = Id FROM [dbo].[AspNetUsers];

                UPDATE [streetcode].[streetcodes]
                SET [UserId] = @FirstUserId
                WHERE [UserId] IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcodes_AspNetUsers_UserId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropIndex(
                name: "IX_streetcodes_UserId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropColumn(
                name: "UserId",
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

            migrationBuilder.Sql(@"
                UPDATE [streetcode].[streetcodes]
                SET [UserId] = NULL;
            ");
        }
    }
}
