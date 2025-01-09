using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Expertise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.AddColumn<string>(
                name: "AboutYourself",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "expertise",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expertise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_expertise",
                schema: "users",
                columns: table => new
                {
                    ExpertiseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_expertise", x => new { x.ExpertiseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_user_expertise_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_expertise_expertise_ExpertiseId",
                        column: x => x.ExpertiseId,
                        principalSchema: "users",
                        principalTable: "expertise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                unique: true,
                filter: "[AvatarId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_user_expertise_UserId",
                schema: "users",
                table: "user_expertise",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_images_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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
                name: "FK_AspNetUsers_images_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_news_images_ImageId",
                schema: "news",
                table: "news");

            migrationBuilder.DropTable(
                name: "user_expertise",
                schema: "users");

            migrationBuilder.DropTable(
                name: "expertise",
                schema: "users");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AboutYourself",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "AspNetUsers");

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
