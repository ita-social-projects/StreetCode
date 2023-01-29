using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "media");

            migrationBuilder.EnsureSchema(
                name: "add_content");

            migrationBuilder.EnsureSchema(
                name: "feedback");

            migrationBuilder.EnsureSchema(
                name: "streetcode");

            migrationBuilder.EnsureSchema(
                name: "timeline");

            migrationBuilder.EnsureSchema(
                name: "partners");

            migrationBuilder.EnsureSchema(
                name: "sources");

            migrationBuilder.EnsureSchema(
                name: "toponyms");

            migrationBuilder.EnsureSchema(
                name: "transactions");

            migrationBuilder.CreateTable(
                name: "donations",
                schema: "feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_donations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "historical_contexts",
                schema: "timeline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historical_contexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "images",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "responses",
                schema: "feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_responses", x => x.Id);
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
                name: "streetcodes",
                schema: "streetcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Teaser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EventStartOrPersonBirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEndOrPersonDeathDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StreetcodeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "add_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "terms",
                schema: "streetcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "timeline_items",
                schema: "timeline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeline_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "toponyms",
                schema: "toponyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Oblast = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdminRegionOld = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AdminRegionNew = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Gromada = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Community = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StreetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_toponyms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "arts",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_arts_images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "media",
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "facts",
                schema: "streetcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FactContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_facts_images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "media",
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partners",
                schema: "partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoId = table.Column<int>(type: "int", nullable: false),
                    IsKeyPartner = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partners_images_LogoId",
                        column: x => x.LogoId,
                        principalSchema: "media",
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "source_link_categories",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_source_link_categories_images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "media",
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audios",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audios_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "related_figures",
                schema: "streetcode",
                columns: table => new
                {
                    ObserverId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_related_figures", x => new { x.ObserverId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_related_figures_streetcodes_ObserverId",
                        column: x => x.ObserverId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_related_figures_streetcodes_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_image",
                schema: "streetcode",
                columns: table => new
                {
                    ImagesId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_image", x => new { x.ImagesId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_image_images_ImagesId",
                        column: x => x.ImagesId,
                        principalSchema: "media",
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_image_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subtitles",
                schema: "add_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subtitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subtitles_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "texts",
                schema: "streetcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_texts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_texts_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_links",
                schema: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QrCodeUrlTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transaction_links_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "videos",
                schema: "media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_videos_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_tag",
                schema: "streetcode",
                columns: table => new
                {
                    StreetcodesId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_tag", x => new { x.StreetcodesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_streetcode_tag_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_tag_tags_TagsId",
                        column: x => x.TagsId,
                        principalSchema: "add_content",
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_timeline_item",
                schema: "streetcode",
                columns: table => new
                {
                    StreetcodesId = table.Column<int>(type: "int", nullable: false),
                    TimelineItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_timeline_item", x => new { x.StreetcodesId, x.TimelineItemsId });
                    table.ForeignKey(
                        name: "FK_streetcode_timeline_item_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_timeline_item_timeline_items_TimelineItemsId",
                        column: x => x.TimelineItemsId,
                        principalSchema: "timeline",
                        principalTable: "timeline_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "timeline_item_historical_context",
                schema: "timeline",
                columns: table => new
                {
                    HistoricalContextsId = table.Column<int>(type: "int", nullable: false),
                    TimelineItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeline_item_historical_context", x => new { x.HistoricalContextsId, x.TimelineItemsId });
                    table.ForeignKey(
                        name: "FK_timeline_item_historical_context_historical_contexts_HistoricalContextsId",
                        column: x => x.HistoricalContextsId,
                        principalSchema: "timeline",
                        principalTable: "historical_contexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timeline_item_historical_context_timeline_items_TimelineItemsId",
                        column: x => x.TimelineItemsId,
                        principalSchema: "timeline",
                        principalTable: "timeline_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coordinates",
                schema: "add_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Longtitude = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CoordinateType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetcodeId = table.Column<int>(type: "int", nullable: true),
                    ToponymId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coordinates_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_coordinates_toponyms_ToponymId",
                        column: x => x.ToponymId,
                        principalSchema: "toponyms",
                        principalTable: "toponyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_toponym",
                schema: "streetcode",
                columns: table => new
                {
                    StreetcodesId = table.Column<int>(type: "int", nullable: false),
                    ToponymsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_toponym", x => new { x.StreetcodesId, x.ToponymsId });
                    table.ForeignKey(
                        name: "FK_streetcode_toponym_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_toponym_toponyms_ToponymsId",
                        column: x => x.ToponymsId,
                        principalSchema: "toponyms",
                        principalTable: "toponyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_art",
                schema: "streetcode",
                columns: table => new
                {
                    StreetcodeId = table.Column<int>(type: "int", nullable: false),
                    ArtId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_art", x => new { x.ArtId, x.StreetcodeId });
                    table.ForeignKey(
                        name: "FK_streetcode_art_arts_ArtId",
                        column: x => x.ArtId,
                        principalSchema: "media",
                        principalTable: "arts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_art_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_fact",
                schema: "streetcode",
                columns: table => new
                {
                    FactsId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_fact", x => new { x.FactsId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_fact_facts_FactsId",
                        column: x => x.FactsId,
                        principalSchema: "streetcode",
                        principalTable: "facts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_fact_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partner_source_links",
                schema: "partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partner_source_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partner_source_links_partners_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "partners",
                        principalTable: "partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streetcode_partners",
                schema: "streetcode",
                columns: table => new
                {
                    PartnersId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_partners", x => new { x.PartnersId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_partners_partners_PartnersId",
                        column: x => x.PartnersId,
                        principalSchema: "partners",
                        principalTable: "partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_partners_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "source_link_subcategories",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceLinkCategoryId = table.Column<int>(type: "int", nullable: false)
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
                name: "streetcode_source_link_categories",
                schema: "sources",
                columns: table => new
                {
                    SourceLinkCategoriesId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_source_link_categories", x => new { x.SourceLinkCategoriesId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoriesId",
                        column: x => x.SourceLinkCategoriesId,
                        principalSchema: "sources",
                        principalTable: "source_link_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_source_link_categories_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "facts",
                columns: new[] { "Id", "FactContent", "ImageId", "Title" },
                values: new object[] { 1, "Навесні 1838-го Карл Брюллов і Василь Жуковський вирішили викупити молодого поета з кріпацтва. Енгельгардт погодився відпустити кріпака за великі гроші — 2500 рублів. Щоб здобути такі гроші, Карл Брюллов намалював портрет Василя Жуковського — вихователя спадкоємця престолу Олександра Миколайовича, і портрет розіграли в лотереї, у якій взяла участь імператорська родина. Лотерея відбулася 4 травня 1838 року, а 7 травня Шевченкові видали відпускну.", null, "Викуп з кріпацтва" });

            migrationBuilder.InsertData(
                schema: "timeline",
                table: "historical_contexts",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Дитинство" },
                    { 2, "Студентство" },
                    { 3, "Життя в Петербурзі" }
                });

            migrationBuilder.InsertData(
                schema: "media",
                table: "images",
                columns: new[] { "Id", "Alt", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "Портрет Тараса Шевченка", "Тарас Шевченко", "http://www.univ.kiev.ua/tpl/img/photo-osobystosti/foto-shevchenko.jpg" },
                    { 2, "Тарас Шевченко: Погруддя жінки", "Погруддя жінки", "https://upload.wikimedia.org/wikipedia/commons/1/10/Taras_Shevchenko_painting_0001.jpg" },
                    { 3, "Тарас Шевченко: Портрет Павла Васильовича Енгельгардта", "Портрет Павла Васильовича Енгельгардта", "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Enhelhard_by_Shevchenko.jpg" },
                    { 4, "Тарас Шевченко: Портрет невідомого", "Портрет невідомого", "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Portret_nevidomoho_Shevchenko_.jpg" },
                    { 5, "Кобзар", "Кобзар", "https://www.megakniga.com.ua/uploads/cache/Products/Product_images_343456/d067b1_w1600.jpg" },
                    { 6, "Мико́ла Костома́ров", "Мико́ла Костома́ров", "https://upload.wikimedia.org/wikipedia/commons/2/21/PGRS_2_051_Kostomarov_-_crop.jpg" },
                    { 7, "Василь Білозерський", "Василь Білозерський", "https://upload.wikimedia.org/wikipedia/commons/6/6a/%D0%91%D0%B5%D0%BB%D0%BE%D0%B7%D0%B5%D1%80%D1%81%D0%BA%D0%B8%D0%B9_%D0%92%D0%B0%D1%81%D0%B8%D0%BB%D0%B8%D0%B9.JPG" },
                    { 8, "Звільнення Херсона", "Звільнення Херсона", "https://img.tsn.ua/cached/907/tsn-15890496c3fba55a55e21f0ca3090d06/thumbs/x/3e/1a/97fe20f34f78c6f13ea84dbf15ee1a3e.jpeg" },
                    { 9, "book", "book", "https://images.unsplash.com/photo-1589998059171-988d887df646?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8b3BlbiUyMGJvb2t8ZW58MHx8MHx8&w=1000&q=80" },
                    { 10, "video", "video", "https://www.earnmydegree.com/sites/all/files/public/video-prod-image.jpg" },
                    { 11, "article", "article", "https://images.laws.com/constitution/constitutional-convention.jpg" },
                    { 12, "SoftServe", "SoftServe", "https://itukraine.org.ua/files/img/illus/members/softserve%20logo.png" },
                    { 13, "Parimatch", "Parimatch", "https://static.ua-football.com/img/upload/19/270071.png" },
                    { 14, "Community Partners", "Community Partners", "https://communitypartnersinc.org/wp-content/uploads/2018/03/CP_Logo_RGB_Horizontal-e1520810390513.png" },
                    { 15, "Володимир-Варфоломей", "Володимир-Варфоломей", "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg/800px-Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg" },
                    { 16, "Леся Українка", "Леся Українка", "https://api.culture.pl/sites/default/files/styles/embed_image_360/public/2022-03/lesya_ukrainka_portrait_public_domain.jpg?itok=1jAIv48D" },
                    { 17, "Іван Мазепа", "Іван Мазепа", "https://reibert.info/attachments/hetmans_catalog-1-4-scaled-jpg.18981447/" },
                    { 18, "Грушевький", "Михайло Грушевський", "/assets/b647ab7ccc32fdb15536.png" },
                    { 19, "Грушевький", "Грушевський", "/assets/46a4e32fed29974d6562.png" },
                    { 20, "Грушевський", "Сучасний Грушевський", "/assets/affcff354ffdf1c788e5.png" },
                    { 21, "мурал", "Мурал Грушевського", "/assets/841a6ad9c34f36476c04.png" },
                    { 22, "Козаки на орбіті", "Козаки на орбіті", "/assets/05b6e5a005600349283a.png" },
                    { 23, "мурал", "Мурал М. Грушевського", "/assets/4153f1a052db8b5bfdd9.png" }
                });

            migrationBuilder.InsertData(
                schema: "feedback",
                table: "responses",
                columns: new[] { "Id", "Description", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Good Job", "dmytrobuchkovsky@gmail.com", null },
                    { 2, "Nice project", "mail@gmail.com", "Dmytro" }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_links",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "Том 2: Суспільно-політичні твори (1907–1914).", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 2, "Том 3: Суспільно-політичні твори (1907 — березень 1917).", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 3, "Том 4. Книга 1: Суспільно-політичні твори (доба Української Центральної Ради, березень 1917 — квітень 1918).", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 4, "Том 5: Історичні студії та розвідки (1888–1896).", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 5, "Том 6: Історичні студії та розвідки (1895–1900).", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 6, "Том 7: Історичні студії та розвідки (1900–1906).", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 7, "Том 8: Історичні студії та розвідки (1906–1916).", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 8, "Том 9: Історичні студії та розвідки (1917–1923).", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 9, "Том 10. Книга 1: Історичні студії та розвідки (1924— 1930)/ упор. О.Юркова.", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 10, "Том 10. Книга 2: Історичні студії та розвідки (1930— 1934)", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" },
                    { 11, "Том 11: Літературно-критичні праці (1883–1931), «По світу»", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 12, "Том 12: Поезія (1882–1903). Проза, драматичні твори, переклади (1883–1886)", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 13, "Том 13 : Серія \"Літературно-критичні та художні твори (1887-1924)\"", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_links",
                columns: new[] { "Id", "Title", "Url" },
                values: new object[] { 14, "Том 14: Рецензії та огляди (1888–1897).", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreatedAt", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "Index", "StreetcodeType", "Teaser", "Title", "ViewCount" },
                values: new object[] { 4, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9329), new DateTime(2022, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "streetcode-event", "Звільнення Херсона (11 листопада 2022) — відвоювання Збройними силами України (ЗСУ) міста Херсона та інших районів Херсонської області та частини Миколаївської області на правому березі Дніпра, тоді як збройні сили РФ Сили відійшли на лівий берег (відомий як відхід росіян з Херсона, 9–11 листопада 2022 р.).", "Звільнення Херсона", 1000 });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreatedAt", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "FirstName", "Index", "LastName", "Rank", "StreetcodeType", "Teaser" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9244), new DateTime(1861, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1814, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Тарас", 1, "Шевченко", "Григорович", "streetcode-person", "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія, Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. Академік Імператорської академії мистецтв" },
                    { 2, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9291), new DateTime(1885, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1817, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Мико́ла", 2, "Костома́ров", "Іва́нович", "streetcode-person", "Мико́ла Іва́нович Костома́ров (4 (16) травня 1817, с. Юрасівка, Острогозький повіт, Воронезька губернія — 7 (19) квітня 1885, Петербург) — видатний український[8][9][10][11][12] історик, етнограф, прозаїк, поет-романтик, мислитель, громадський діяч, етнопсихолог[13][14][15]. \r\n\r\nБув співзасновником та активним учасником слов'янофільсько-українського київського об'єднання «Кирило - Мефодіївське братство». У 1847 році за участь в українофільському братстві Костомарова арештовують та перевозять з Києва до Петербурга,де він і провів решту свого життя." },
                    { 3, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9295), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Василь", 3, "Білозерський", "Михайлович", "streetcode-person", "Білозерський Василь Михайлович (1825, хутір Мотронівка, Чернігівщина — 20 лютого (4 березня) 1899) — український громадсько-політичний і культурний діяч, журналіст." },
                    { 5, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9299), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Володимир-Варфоломей", 5, "Кропивницький-Шевченківський", null, "streetcode-person", "some teaser" },
                    { 6, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9302), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Леся", 6, "Українка", null, "streetcode-person", "some teaser" },
                    { 7, new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9306), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Іван", 7, "Мазепа", null, "streetcode-person", "some teaser" }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "tags",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "writer" },
                    { 2, "artist" },
                    { 3, "composer" },
                    { 4, "wictory" },
                    { 5, "Наукова школа" },
                    { 6, "Історія" },
                    { 7, "Політика" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "terms",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Етнографія — суспільствознавча наука, об'єктом дослідження якої є народи, їхня культура і побут, походження, розселення, процеси культурно-побутових відносин на всіх етапах історії людства.", "етнограф" },
                    { 2, "Гра́фіка — вид образотворчого мистецтва, для якого характерна перевага ліній і штрихів, використання контрастів білого та чорного та менше, ніж у живописі, використання кольору. Твори можуть мати як монохромну, так і поліхромну гаму.", "гравер" },
                    { 3, "Кріпа́цтво, або кріпосне́ право, у вузькому сенсі — правова система, або система правових норм при феодалізмі, яка встановлювала залежність селянина від феодала й неповну власність феодала на селянина.", "кріпак" },
                    { 4, "Ма́чуха — нерідна матір для дітей чоловіка від його попереднього шлюбу.", "мачуха" }
                });

            migrationBuilder.InsertData(
                schema: "timeline",
                table: "timeline_items",
                columns: new[] { "Id", "Date", "Description", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(1831, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 2, new DateTime(1830, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Засвідчивши свою відпускну в петербурзькій Палаті цивільного суду, Шевченко став учнем Академії мистецтв, де його наставником став К. Брюллов. За словами Шевченка: «настала найсвітліша доба його життя, незабутні, золоті дні» навчання в Академії мистецтв, яким він присвятив у 1856 році автобіографічну повість «Художник».", "Учень Петербурзької академії мистецтв" },
                    { 3, new DateTime(1832, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 4, new DateTime(1833, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 5, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 6, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 7, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 8, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 9, new DateTime(1835, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 10, new DateTime(1836, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" }
                });

            migrationBuilder.InsertData(
                schema: "toponyms",
                table: "toponyms",
                columns: new[] { "Id", "AdminRegionNew", "AdminRegionOld", "Community", "Gromada", "Oblast", "StreetName", "StreetType" },
                values: new object[,]
                {
                    { 1, null, null, null, null, "Seed1", "SeedStreet1", null },
                    { 2, null, null, null, null, "Seed2", "SeedStreet2", null },
                    { 3, null, null, null, null, "Seed3", "SeedStreet3", null }
                });

            migrationBuilder.InsertData(
                schema: "media",
                table: "arts",
                columns: new[] { "Id", "Description", "ImageId" },
                values: new object[,]
                {
                    { 1, "Анатолій Федірко, «Український супрематичний політичний діяч Михайло Грушевський», 2019-2020 роки.", 18 },
                    { 2, null, 19 },
                    { 3, "Назар Дубів опублікував серію малюнків, у яких перетворив класиків української літератури та політичних діячів на сучасних модників", 20 },
                    { 4, null, 21 },
                    { 5, "«Козаки на орбіті» поєднує не тільки тему козаків, а й апелює до космічної тематики.", 22 },
                    { 6, "На вулиці Січових стрільців, 75 закінчили малювати мурал Михайла Грушевського на місці малюнка будинку з лелекою.", 23 }
                });

            migrationBuilder.InsertData(
                schema: "media",
                table: "audios",
                columns: new[] { "Id", "Description", "StreetcodeId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "for streetcode1", 1, "audio1", "https://somelink1" },
                    { 2, "for streetcode2", 2, "audio2", "https://somelink2" },
                    { 3, "for streetcode3", 3, "audio3", "https://somelink3" },
                    { 4, "for streetcode4", 4, "audio4", "https://somelink4" }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "coordinates",
                columns: new[] { "Id", "CoordinateType", "Latitude", "Longtitude", "StreetcodeId" },
                values: new object[,]
                {
                    { 6, "coordinate_streetcode", 49.8429m, 24.0311m, 1 },
                    { 7, "coordinate_streetcode", 50.4550m, 30.5238m, 2 },
                    { 9, "coordinate_streetcode", 50.4690m, 30.5328m, 3 },
                    { 10, "coordinate_streetcode", 46.3950m, 32.3738m, 4 }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "facts",
                columns: new[] { "Id", "FactContent", "ImageId", "Title" },
                values: new object[] { 2, " Ознайомившись випадково з рукописними творами Шевченка й вражений ними, П. Мартос виявив до них великий інтерес. Він порадився із Є. Гребінкою і запропонував Шевченку видати їх окремою книжкою, яку згодом назвали «Кобзарем».", 5, "Перший Кобзар" });

            migrationBuilder.InsertData(
                schema: "partners",
                table: "partners",
                columns: new[] { "Id", "Description", "IsKeyPartner", "LogoId", "TargetUrl", "Title" },
                values: new object[] { 1, "Український культурний фонд є флагманською українською інституцією культури, яка у своїй діяльності інтегрує різні види мистецтва – від сучасного мистецтва, нової музики й театру до літератури та музейної справи. Мистецький арсенал є флагманською українською інституцією культури, яка у своїй діяльності інтегрує різні види мистецтва – від сучасного мистецтва, нової музики й театру до літератури та музейної справи.", true, 12, "https://www.softserveinc.com/en-us", "SoftServe" });

            migrationBuilder.InsertData(
                schema: "partners",
                table: "partners",
                columns: new[] { "Id", "Description", "LogoId", "TargetUrl", "Title" },
                values: new object[,]
                {
                    { 2, "some text", 13, "https://parimatch.com/", "Parimatch" },
                    { 3, null, 14, "https://partners.salesforce.com/pdx/s/?language=en_US&redirected=RGSUDODQUL", "comunity partner" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "related_figures",
                columns: new[] { "ObserverId", "TargetId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 3 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_link_categories",
                columns: new[] { "Id", "ImageId", "Title" },
                values: new object[,]
                {
                    { 1, 9, "Книги" },
                    { 2, 10, "Фільми" },
                    { 3, 11, "Цитати" }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "subtitles",
                columns: new[] { "Id", "Description", "FirstName", "LastName", "Status", "StreetcodeId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "description", "Dmytro", "Buchkovsky", (byte)0, 1, null, "https://t.me/MaisterD" },
                    { 2, "description", "Dmytro", "Buchkovsky", (byte)1, 2, null, "https://t.me/MaisterD" },
                    { 3, "description", "Dmytro", "Buchkovsky", (byte)0, 3, null, "https://t.me/MaisterD" },
                    { 4, "description", "Oleksndr", "Lazarenko", (byte)0, 1, null, null },
                    { 5, null, "Oleksndr", "Lazarenko", (byte)0, 2, null, null },
                    { 6, null, "Yaroslav", "Chushenko", (byte)1, 1, null, null },
                    { 7, null, "Yaroslav", "Chushenko", (byte)1, 3, null, null },
                    { 8, null, "Nazarii", "Hovdysh", (byte)0, 4, null, null },
                    { 9, null, "Tatiana", "Shumylo", (byte)1, 4, null, null }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "texts",
                columns: new[] { "Id", "StreetcodeId", "TextContent", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Тарас Шевченко народився 9 березня 1814 року в селі Моринці Пединівської волості Звенигородського повіту Київської губернії. Був третьою дитиною селян-кріпаків Григорія Івановича Шевченка та Катерини Якимівни після сестри Катерини (1804 — близько 1848) та брата Микити (1811 — близько 1870).\r\n\r\n                    За родинними переказами, Тарасові діди й прадіди з батьківського боку походили від козака Андрія, який на початку XVIII століття прийшов із Запорізької Січі. Батьки його матері, Катерини Якимівни Бойко, були переселенцями з Прикарпаття.\r\n\r\n                    1816 року сім'я Шевченків переїхала до села Кирилівка Звенигородського повіту, звідки походив Григорій Іванович. Дитячі роки Тараса пройшли в цьому селі. 1816 року народилася Тарасова сестра Ярина, 1819 року — сестра Марія, а 1821 року народився Тарасів брат Йосип.\r\n\r\n                    Восени 1822 року Тарас Шевченко почав учитися грамоти у дяка Совгиря. Тоді ж ознайомився з творами Григорія Сковороди.\r\n\r\n                    10 лютого 1823 року його старша сестра Катерина вийшла заміж за Антона Красицького — селянина із Зеленої Діброви, а 1 вересня 1823 року від тяжкої праці й злиднів померла мати Катерина. \r\n\r\n                    19 жовтня 1823 року батько одружився вдруге з удовою Оксаною Терещенко, в якої вже було троє дітей. Вона жорстоко поводилася з нерідними дітьми, зокрема з малим Тарасом. 1824 року народилася Тарасова сестра Марія — від другого батькового шлюбу.\r\n\r\n                    Хлопець чумакував із батьком. Бував у Звенигородці, Умані, Єлисаветграді (тепер Кропивницький). 21 березня (2 квітня) 1825 року батько помер, і невдовзі мачуха повернулася зі своїми трьома дітьми до Моринців. Зрештою Тарас змушений був залишити домівку. Деякий час Тарас жив у свого дядька Павла, який після смерті його батька став опікуном сиріт. Дядько Павло був «великий катюга»; Тарас працював у нього, разом із наймитом у господарстві, але у підсумку не витримав тяжких умов життя й пішов у найми до нового кирилівського дяка Петра Богорського.\r\n\r\n                    Як попихач носив воду, опалював школу, обслуговував дяка, читав псалтир над померлими і вчився. Не стерпівши знущань Богорського й відчуваючи великий потяг до живопису, Тарас утік від дяка й почав шукати в навколишніх селах учителя-маляра. Кілька днів наймитував і «вчився» малярства в диякона Єфрема. Також мав учителів-малярів із села Стеблева, Канівського повіту та із села Тарасівки Звенигородського повіту. 1827 року він пас громадську отару в Кирилівці й там зустрічався з Оксаною Коваленко. Згодом подругу свого дитинства поет не раз згадає у своїх творах і присвятить їй поему «Мар'яна-черниця».", "Дитинство та юність" },
                    { 2, 2, "Батьки М. І. Костомарова намагалися прищепити сину вільнолюбні ідеї і дати добру освіту. Тому вже з 10 років М. Костомарова відправили навчатися до Московського пансіону, а згодом до Воронезької гімназії, яку той закінчив 1833 р.\r\n\r\n1833 р. М. І. Костомаров вступає на історико-філологічний факультет Харківського університету. Вже у цьому навчальному закладі він проявив непересічні здібності до навчання.\r\n\r\nВ університеті Микола Костомаров вивчав стародавні й нові мови, цікавився античною історією, німецькою філософією і новою французькою літературою, учився грати на фортепіано, пробував писати вірші. Зближення з гуртком українських романтиків Харківського університету незабаром визначило його захоплення переважно фольклором і козацьким минулим України.\r\n\r\nУ ті роки у Харківському університеті навколо професора-славіста і літератора-романтика І. Срезневського сформувався гурток студентів, захоплених збиранням зразків української народної пісенної творчості. Вони сприймали фольклор як вираження народного духу, самі складали вірші, балади і ліричні пісні, звертаючись до народної творчості.\r\n\r\nКостомаров в університетські роки дуже багато читав. Перевантаження позначилося на його здоров'ї — ще за студентства значно погіршився зір.\r\n\r\nНа світогляд М. І. Костомарова вплинули професор грецької літератури Харківського університету А. О. Валицький та професор всесвітньої історії М. М. Лунін.\r\n\r\n1836 р. М. І. Костомаров закінчив університет, а в січні 1837 р. склав іспити на ступінь кандидата й отримав направлення у Кінбурнський 7-й драгунський полк юнкером.\r\n\r\nУ січні 1837 року Костомаров склав іспити з усіх предметів, і 8 грудня 1837 року його затвердили в статусі кандидата.", "Юність і навчання" },
                    { 3, 3, "Народився у дворянській родині на хуторі Мотронівка (нині у межах с. Оленівка поблизу Борзни).\r\n\r\nУ 1843–1846 роках здобув вищу освіту на історико-філологічному факультеті Київського Імператорського університету св. Володимира.\r\n\r\n1846–1847 — учитель Петровського кадетського корпусу у Полтаві.\r\n\r\nРазом з М. Костомаровим і М. Гулаком був організатором Кирило-Мефодіївського братства. Брав участь у створенні «Статуту Слов'янського братства св. Кирила і Мефодія». Автор «Записки» — пояснень до статуту братства. Розвивав ідеї християнського соціалізму, виступав за об'єднання всіх слов'янських народів у республіканську федерацію, в якій провідну роль відводив Україні.\r\n1847 — 10 квітня був заарештований у Варшаві. Засланий до Олонецької губернії під нагляд поліції. Служив у Петрозаводському губернському правлінні.\r\n\r\n1856 — звільнений із заслання. Оселився у Санкт-Петербурзі, де став активним членом місцевого гуртка українців.\r\n\r\n1861–1862 — редактор першого українського щомісячного журналу «Основа».\r\n\r\nЗгодом служив у Варшаві. Підтримував зв'язки з Галичиною, співпрацював у часописах «Мета» і «Правда».\r\n\r\nОстанні роки життя провів на хуторі Мотронівці.", "Життєпис" },
                    { 4, 4, "Експерти пояснили, що дасть херсонська перемога українським силам\r\n\r\nНа тлі заяв окупантів про відведення військ та сил рф від Херсона та просування ЗСУ на херсонському напрямку українські бійці можуть отримати вогневий контроль над найважливішими дорогами Криму. Більше того, звільнення облцентру переріже постачання зброї для росії.", "визволення Херсона" }
                });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "transaction_links",
                columns: new[] { "Id", "QrCodeUrl", "QrCodeUrlTitle", "StreetcodeId", "Url", "UrlTitle" },
                values: new object[] { 1, "https://qrcode/1", null, 1, "https://streetcode/1", null });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "transaction_links",
                columns: new[] { "Id", "QrCodeUrl", "QrCodeUrlTitle", "StreetcodeId", "Url", "UrlTitle" },
                values: new object[,]
                {
                    { 2, "https://qrcode/2", null, 2, "https://streetcode/2", null },
                    { 3, "https://qrcode/3", null, 3, "https://streetcode/3", null },
                    { 4, "https://qrcode/4", null, 4, "https://streetcode/4", null }
                });

            migrationBuilder.InsertData(
                schema: "media",
                table: "videos",
                columns: new[] { "Id", "Description", "StreetcodeId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "for streetcode1", 2, "audio1", "https://somelink1" },
                    { 2, null, 1, "Біографія Т.Г.Шевченка", "https://www.youtube.com/watch?v=VVFEi6lTpZk&ab_channel=%D0%9E%D1%81%D1%82%D0%B0%D0%BD%D0%BD%D1%96%D0%B9%D0%93%D0%B5%D1%82%D1%8C%D0%BC%D0%B0%D0%BD" },
                    { 3, "За виконанням Богдана Ступки", 1, "Вірш: Мені Однаково", "https://www.youtube.com/watch?v=f55dHPEY-0U&ab_channel=%D0%86%D0%B3%D0%BE%D1%80%D0%9E%D0%BF%D0%B0%D1%86%D1%8C%D0%BA%D0%B8%D0%B9" },
                    { 4, "За виконанням Богдана Ступки", 4, "Вірш: Мені Однаково", "https://youtu.be/v3siIQi4nCQ" }
                });

            migrationBuilder.InsertData(
                schema: "partners",
                table: "partner_source_links",
                columns: new[] { "Id", "LogoUrl", "PartnerId", "TargetUrl", "Title" },
                values: new object[,]
                {
                    { 1, "https://play-lh.googleusercontent.com/kMofEFLjobZy_bCuaiDogzBcUT-dz3BBbOrIEjJ-hqOabjK8ieuevGe6wlTD15QzOqw", 1, "https://www.linkedin.com/company/softserve/", "LinkedIn" },
                    { 2, "https://www.facebook.com/images/fb_icon_325x325.png", 1, "https://www.instagram.com/softserve_people/", "Instagram" },
                    { 3, "https://upload.wikimedia.org/wikipedia/commons/thumb/9/95/Instagram_logo_2022.svg/1200px-Instagram_logo_2022.svg.png", 1, "https://www.facebook.com/SoftServeCompany", "facebook" }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_link_subcategories",
                columns: new[] { "Id", "SourceLinkCategoryId", "Title" },
                values: new object[,]
                {
                    { 1, 2, "Фільми про Т. Г. Шевченко" },
                    { 2, 2, "Хроніки про Т. Г. Шевченко" },
                    { 3, 2, "Блоги про Т. Г. Шевченко" },
                    { 4, 1, "Праці Грушевського" },
                    { 5, 1, "Книги про Грушевського" },
                    { 6, 1, "Статті" },
                    { 7, 3, "Пряма мова" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeId", "Index" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 3 },
                    { 2, 2, 7 },
                    { 2, 3, 6 },
                    { 3, 1, 4 },
                    { 3, 3, 2 },
                    { 4, 1, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_arts_ImageId",
                schema: "media",
                table: "arts",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audios_StreetcodeId",
                schema: "media",
                table: "audios",
                column: "StreetcodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coordinates_StreetcodeId",
                schema: "add_content",
                table: "coordinates",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_coordinates_ToponymId",
                schema: "add_content",
                table: "coordinates",
                column: "ToponymId",
                unique: true,
                filter: "[ToponymId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_facts_ImageId",
                schema: "streetcode",
                table: "facts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_partner_source_links_PartnerId",
                schema: "partners",
                table: "partner_source_links",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_partners_LogoId",
                schema: "partners",
                table: "partners",
                column: "LogoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_related_figures_TargetId",
                schema: "streetcode",
                table: "related_figures",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_source_link_categories_ImageId",
                schema: "sources",
                table: "source_link_categories",
                column: "ImageId");

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
                name: "IX_streetcode_art_ArtId_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeId" });

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_art_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_art",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_fact_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_fact",
                column: "StreetcodesId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_image_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_image",
                column: "StreetcodesId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_partners_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_partners",
                column: "StreetcodesId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_source_link_categories_StreetcodesId",
                schema: "sources",
                table: "streetcode_source_link_categories",
                column: "StreetcodesId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_tag_TagsId",
                schema: "streetcode",
                table: "streetcode_tag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_timeline_item_TimelineItemsId",
                schema: "streetcode",
                table: "streetcode_timeline_item",
                column: "TimelineItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_toponym_ToponymsId",
                schema: "streetcode",
                table: "streetcode_toponym",
                column: "ToponymsId");

            migrationBuilder.CreateIndex(
                name: "IX_subtitles_StreetcodeId",
                schema: "add_content",
                table: "subtitles",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_texts_StreetcodeId",
                schema: "streetcode",
                table: "texts",
                column: "StreetcodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_timeline_item_historical_context_TimelineItemsId",
                schema: "timeline",
                table: "timeline_item_historical_context",
                column: "TimelineItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_links_StreetcodeId",
                schema: "transactions",
                table: "transaction_links",
                column: "StreetcodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_videos_StreetcodeId",
                schema: "media",
                table: "videos",
                column: "StreetcodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audios",
                schema: "media");

            migrationBuilder.DropTable(
                name: "coordinates",
                schema: "add_content");

            migrationBuilder.DropTable(
                name: "donations",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "partner_source_links",
                schema: "partners");

            migrationBuilder.DropTable(
                name: "related_figures",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "responses",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "source_link_source_link_subcategory",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "streetcode_art",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_fact",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_image",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_partners",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_source_link_categories",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "streetcode_tag",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_timeline_item",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_toponym",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "subtitles",
                schema: "add_content");

            migrationBuilder.DropTable(
                name: "terms",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "texts",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "timeline_item_historical_context",
                schema: "timeline");

            migrationBuilder.DropTable(
                name: "transaction_links",
                schema: "transactions");

            migrationBuilder.DropTable(
                name: "videos",
                schema: "media");

            migrationBuilder.DropTable(
                name: "source_link_subcategories",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "source_links",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "arts",
                schema: "media");

            migrationBuilder.DropTable(
                name: "facts",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "partners",
                schema: "partners");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "add_content");

            migrationBuilder.DropTable(
                name: "toponyms",
                schema: "toponyms");

            migrationBuilder.DropTable(
                name: "historical_contexts",
                schema: "timeline");

            migrationBuilder.DropTable(
                name: "timeline_items",
                schema: "timeline");

            migrationBuilder.DropTable(
                name: "streetcodes",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "source_link_categories",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "images",
                schema: "media");
        }
    }
}
