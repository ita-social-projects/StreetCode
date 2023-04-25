using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class UpdateSourceEntitiesAndAudio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_audios_streetcodes_StreetcodeId",
                schema: "media",
                table: "audios");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoriesId",
                schema: "sources",
                table: "streetcode_source_link_categories");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_source_link_categories_streetcodes_StreetcodesId",
                schema: "sources",
                table: "streetcode_source_link_categories");

            migrationBuilder.DropTable(
                name: "source_link_source_link_subcategory",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "source_link_subcategories",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "source_links",
                schema: "sources");

            migrationBuilder.DropIndex(
                name: "IX_audios_StreetcodeId",
                schema: "media",
                table: "audios");

            migrationBuilder.DropColumn(
                name: "StreetcodeId",
                schema: "media",
                table: "audios");

            migrationBuilder.RenameColumn(
                name: "StreetcodesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                newName: "StreetcodeId");

            migrationBuilder.RenameColumn(
                name: "SourceLinkCategoriesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                newName: "SourceLinkCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_streetcode_source_link_categories_StreetcodesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                newName: "IX_streetcode_source_link_categories_StreetcodeId");

            migrationBuilder.AlterColumn<string>(
                name: "SubtitleText",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AudioId",
                schema: "streetcode",
                table: "streetcodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                schema: "sources",
                table: "streetcode_source_link_categories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_streetcodes_AudioId",
                schema: "streetcode",
                table: "streetcodes",
                column: "AudioId",
                unique: true,
                filter: "[AudioId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoryId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                column: "SourceLinkCategoryId",
                principalSchema: "sources",
                principalTable: "source_link_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_source_link_categories_streetcodes_StreetcodeId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcodes_audios_AudioId",
                schema: "streetcode",
                table: "streetcodes",
                column: "AudioId",
                principalSchema: "media",
                principalTable: "audios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoryId",
                schema: "sources",
                table: "streetcode_source_link_categories");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcode_source_link_categories_streetcodes_StreetcodeId",
                schema: "sources",
                table: "streetcode_source_link_categories");

            migrationBuilder.DropForeignKey(
                name: "FK_streetcodes_audios_AudioId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropTable(
                name: "streetcode_tag_index",
                schema: "add_content");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Users");

            migrationBuilder.DropIndex(
                name: "IX_streetcodes_AudioId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropColumn(
                name: "DateViewPattern",
                schema: "timeline",
                table: "timeline_items");

            migrationBuilder.DropColumn(
                name: "AudioId",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropColumn(
                name: "Text",
                schema: "sources",
                table: "streetcode_source_link_categories");

            migrationBuilder.DropColumn(
                name: "IsVisibleEverywhere",
                schema: "partners",
                table: "partners");

            migrationBuilder.RenameColumn(
                name: "StreetcodeId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                newName: "StreetcodesId");

            migrationBuilder.RenameColumn(
                name: "SourceLinkCategoryId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                newName: "SourceLinkCategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_streetcode_source_link_categories_StreetcodeId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                newName: "IX_streetcode_source_link_categories_StreetcodesId");

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeId",
                schema: "media",
                table: "audios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "source_link_subcategories",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceLinkCategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_source_link_subcategories_source_link_categories_SourceLinkCategoryId",
                        column: x => x.SourceLinkCategoryId,
                        principalSchema: "sources",
                        principalTable: "source_link_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "source_links",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_links", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "source_link_source_link_subcategory",
                schema: "sources",
                columns: table => new
                {
                    SourceLinksId = table.Column<int>(type: "int", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_source_link_subcategory", x => new { x.SourceLinksId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_source_link_source_link_subcategory_source_link_subcategories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalSchema: "sources",
                        principalTable: "source_link_subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_source_link_source_link_subcategory_source_links_SourceLinksId",
                        column: x => x.SourceLinksId,
                        principalSchema: "sources",
                        principalTable: "source_links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audios_StreetcodeId",
                schema: "media",
                table: "audios",
                column: "StreetcodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_source_link_source_link_subcategory_SubCategoriesId",
                schema: "sources",
                table: "source_link_source_link_subcategory",
                column: "SubCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_source_link_subcategories_SourceLinkCategoryId",
                schema: "sources",
                table: "source_link_subcategories",
                column: "SourceLinkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_tag_TagsId",
                schema: "streetcode",
                table: "streetcode_tag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_audios_streetcodes_StreetcodeId",
                schema: "media",
                table: "audios",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoriesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                column: "SourceLinkCategoriesId",
                principalSchema: "sources",
                principalTable: "source_link_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_streetcode_source_link_categories_streetcodes_StreetcodesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                column: "StreetcodesId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
