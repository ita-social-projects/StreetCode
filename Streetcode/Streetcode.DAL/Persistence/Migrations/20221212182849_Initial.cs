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
                name: "streetcode");

            migrationBuilder.EnsureSchema(
                name: "timeline");

            migrationBuilder.EnsureSchema(
                name: "partner_sponsors");

            migrationBuilder.EnsureSchema(
                name: "feedback");

            migrationBuilder.EnsureSchema(
                name: "sources");

            migrationBuilder.EnsureSchema(
                name: "toponyms");

            migrationBuilder.EnsureSchema(
                name: "transactions");

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
                name: "partners",
                schema: "partner_sponsors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "responses",
                schema: "feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_responses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "source_link_categories",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_link_categories", x => x.Id);
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
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EventStartOrPersonBirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEndOrPersonDeathDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    streetcode_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    Description = table.Column<string>(type: "text", nullable: true)
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
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "partner_source_links",
                schema: "partner_sponsors",
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
                        principalSchema: "partner_sponsors",
                        principalTable: "partners",
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
                name: "source_links",
                schema: "sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_source_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_source_links_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
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
                name: "streetcode_partners",
                schema: "partner_sponsors",
                columns: table => new
                {
                    StreetcodeId = table.Column<int>(type: "int", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    IsSponsor = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_partners", x => new { x.PartnerId, x.StreetcodeId });
                    table.ForeignKey(
                        name: "FK_streetcode_partners_partners_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "partner_sponsors",
                        principalTable: "partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_partners_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Description = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
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
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    coordinate_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "streetcode_arts",
                schema: "streetcode",
                columns: table => new
                {
                    ArtsId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_arts", x => new { x.ArtsId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_arts_arts_ArtsId",
                        column: x => x.ArtsId,
                        principalSchema: "media",
                        principalTable: "arts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_arts_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
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
                name: "SourceLinkSourceLinkCategory",
                schema: "sources",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    SourceLinksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceLinkSourceLinkCategory", x => new { x.CategoriesId, x.SourceLinksId });
                    table.ForeignKey(
                        name: "FK_SourceLinkSourceLinkCategory_source_link_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "sources",
                        principalTable: "source_link_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SourceLinkSourceLinkCategory_source_links_SourceLinksId",
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
                    { 1, "Book" },
                    { 2, "Wideo" },
                    { 3, "Article" }
                });

            migrationBuilder.InsertData(
                schema: "media",
                table: "images",
                columns: new[] { "Id", "Alt", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "Портрет Тараса Шевченка", "Тарас Шевченко", "https://www.bing.com/images/search?view=detailV2&ccid=07Ymnt6l&id=A8ACFBEB6A3695B1E7DD2887A46505D759921BDC&thid=OIP.07Ymnt6ljB86Jx-Fy2wGUwHaKY&mediaurl=https%3A%2F%2Fproprikol.ru%2Fwp-content%2Fuploads%2F2021%2F05%2Fkartinki-taras-shevchenko-39.jpg&cdnurl=https%3A%2F%2Fth.bing.com%2Fth%2Fid%2FR.d3b6269edea58c1f3a271f85cb6c0653%3Frik%3D3BuSWdcFZaSHKA%26pid%3DImgRaw%26r%3D0&exph=1581&expw=1128&q=%d1%82%d0%b0%d1%80%d0%b0%d1%81+%d1%88%d0%b5%d0%b2%d1%87%d0%b5%d0%bd%d0%ba%d0%be&simid=608002717784548528&form=IRPRST&ck=0EF4CC2DA9612AC10DAE96953F62051F&selectedindex=1&ajaxhist=0&ajaxserp=0&vt=0&sim=11" },
                    { 2, "Тарас Шевченко: Погруддя жінки", "Погруддя жінки", "https://upload.wikimedia.org/wikipedia/commons/1/10/Taras_Shevchenko_painting_0001.jpg" },
                    { 3, "Тарас Шевченко: Портрет Павла Васильовича Енгельгардта", "Портрет Павла Васильовича Енгельгардта", "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Enhelhard_by_Shevchenko.jpg" },
                    { 4, "Тарас Шевченко: Портрет невідомого", "Портрет невідомого", "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Portret_nevidomoho_Shevchenko_.jpg" },
                    { 5, "Кобзар", "Кобзар", "https://www.bing.com/images/search?view=detailV2&ccid=6juPycgD&id=00A2C7B1F325A9870421D651A956BCE2C851654E&thid=OIP.6juPycgDNwJ3v2Zr-kde1gHaK_&mediaurl=https%3A%2F%2Fwww.megakniga.com.ua%2Fuploads%2Fcache%2FProducts%2FProduct_images_343456%2Fd067b1_w1600.jpg&cdnurl=https%3A%2F%2Fth.bing.com%2Fth%2Fid%2FR.ea3b8fc9c803370277bf666bfa475ed6%3Frik%3DTmVRyOK8VqlR1g%26pid%3DImgRaw%26r%3D0&exph=1200&expw=809&q=%d0%ba%d0%be%d0%b1%d0%b7%d0%b0%d1%80&simid=608047540067197142&form=IRPRST&ck=4280C365AEBC65D796FBF885B3252710&selectedindex=1&ajaxhist=0&ajaxserp=0&vt=0&sim=11" },
                    { 6, "Мико́ла Костома́ров", "Мико́ла Костома́ров", "https://www.bing.com/images/search?view=detailV2&ccid=KUJZwRaU&id=A53DDEBFF57BE2396FB7FA50737F83704B1BE30F&thid=OIP.KUJZwRaUjipKMLR8H91BrAAAAA&mediaurl=https%3a%2f%2fgdb.rferl.org%2f224F2B76-EE74-4B85-A78A-BF8A354FA0B1_w250_r0_s.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.294259c116948e2a4a30b47c1fdd41ac%3frik%3dD%252bMbS3CDf3NQ%252bg%26pid%3dImgRaw%26r%3d0&exph=340&expw=250&q=%d0%9c%d0%b8%d0%ba%d0%be%cc%81%d0%bb%d0%b0+%d0%9a%d0%be%d1%81%d1%82%d0%be%d0%bc%d0%b0%cc%81%d1%80%d0%be%d0%b2&simid=608030609289524022&FORM=IRPRST&ck=E08972A7A7E2CEE9B67158DDC372F92F&selectedIndex=3&ajaxhist=0&ajaxserp=0" },
                    { 7, "Василь Білозерський", "Василь Білозерський", "https://www.bing.com/images/search?view=detailV2&ccid=hIQUFjAM&id=B14676F51B4A0EB314ED15283540D088B3030E28&thid=OIP.hIQUFjAMGwOt7f7ujR44aQAAAA&mediaurl=https%3a%2f%2fnaurok-test.nyc3.cdn.digitaloceanspaces.com%2fuploads%2ftest%2f229691%2f36505%2f276576_1582512990.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.84841416300c1b03adedfeee8d1e3869%3frik%3dKA4Ds4jQQDUoFQ%26pid%3dImgRaw%26r%3d0&exph=351&expw=240&q=%d0%92%d0%b0%d1%81%d0%b8%d0%bb%d1%8c+%d0%91%d1%96%d0%bb%d0%be%d0%b7%d0%b5%d1%80%d1%81%d1%8c%d0%ba%d0%b8%d0%b9&simid=608001205960330039&FORM=IRPRST&ck=07DE282212732F4C0712D614C87002F3&selectedIndex=1&ajaxhist=0&ajaxserp=0" },
                    { 8, "Звільнення Херсона", "Звільнення Херсона", "https://www.bing.com/images/search?view=detailV2&ccid=F5o3vrW9&id=5409686EF1396243251CE5AF505766A0A2D0662E&thid=OIP.F5o3vrW9jZJ9ECMgkmevTwHaFj&mediaurl=https%3a%2f%2fstorage1.censor.net%2fimages%2f1%2f7%2f9%2fa%2f179a37beb5bd8d927d1023209267af4f%2foriginal.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.179a37beb5bd8d927d1023209267af4f%3frik%3dLmbQoqBmV1Cv5Q%26pid%3dImgRaw%26r%3d0&exph=720&expw=960&q=%d0%b2%d0%b8%d0%b7%d0%b2%d0%be%d0%bb%d0%b5%d0%bd%d0%bd%d1%8f+%d1%85%d0%b5%d1%80%d1%81%d0%be%d0%bd%d1%83&simid=608050323200235844&FORM=IRPRST&ck=C9A86B9D5EBBADF456F315DFD0BA990B&selectedIndex=3&ajaxhist=0&ajaxserp=0" }
                });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "partners",
                columns: new[] { "Id", "Description", "LogoUrl", "TargetUrl", "Title" },
                values: new object[,]
                {
                    { 1, "Developers", "https://www.bing.com/images/search?view=detailV2&ccid=g3DnkGqg&id=98C6F1FDD6CDA685A3DE2AD392FAC228180A28CC&thid=OIP.g3DnkGqgmhKFWM2ct5mXrAHaHa&mediaurl=https%3a%2f%2fyt3.ggpht.com%2fa-%2fAN66SAxaiWXvFxW9BUQ32pzQ5tv5UuXz2fLZ20LaMg%3ds900-mo-c-c0xffffffff-rj-k-no&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.8370e7906aa09a128558cd9cb79997ac%3frik%3dzCgKGCjC%252bpLTKg%26pid%3dImgRaw%26r%3d0&exph=900&expw=900&q=softserve&simid=608013145967840441&FORM=IRPRST&ck=C08BED6E397D35D8A4824BB4B78EBCE8&selectedIndex=1&ajaxhist=0&ajaxserp=0", "https://www.softserveinc.com/en-us", "SoftServe" },
                    { 2, null, "https://www.bing.com/images/search?view=detailV2&ccid=9TObzn%2ba&id=21D8755FE7846CE9660BC2365F5EE70417D31DA7&thid=OIP.9TObzn-a15MsLhdfHh1e_gHaE8&mediaurl=https%3a%2f%2fi2.wp.com%2feuropeangaming.eu%2fportal%2fwp-content%2fuploads%2f2020%2f02%2f5-10.jpg%3ffit%3d1200%252C800%26ssl%3d1&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.f5339bce7f9ad7932c2e175f1e1d5efe%3frik%3dpx3TFwTnXl82wg%26pid%3dImgRaw%26r%3d0&exph=800&expw=1200&q=parimatch&simid=607987165708116105&FORM=IRPRST&ck=BA1164F39CC2BBD1CE20F50A93602E5C&selectedIndex=1&ajaxhist=0&ajaxserp=0", "https://parimatch.com/", "parimatch" },
                    { 3, null, "https://www.bing.com/images/search?view=detailV2&ccid=9TObzn%2ba&id=21D8755FE7846CE9660BC2365F5EE70417D31DA7&thid=OIP.9TObzn-a15MsLhdfHh1e_gHaE8&mediaurl=https%3a%2f%2fi2.wp.com%2feuropeangaming.eu%2fportal%2fwp-content%2fuploads%2f2020%2f02%2f5-10.jpg%3ffit%3d1200%252C800%26ssl%3d1&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.f5339bce7f9ad7932c2e175f1e1d5efe%3frik%3dpx3TFwTnXl82wg%26pid%3dImgRaw%26r%3d0&exph=800&expw=1200&q=parimatch&simid=607987165708116105&FORM=IRPRST&ck=BA1164F39CC2BBD1CE20F50A93602E5C&selectedIndex=1&ajaxhist=0&ajaxserp=0", "https://parimatch.com/", "comunity partner" }
                });

            migrationBuilder.InsertData(
                schema: "feedback",
                table: "responses",
                columns: new[] { "Id", "Description", "Email", "FirstName" },
                values: new object[,]
                {
                    { 1, "Good Job", "dmytrobuchkovsky@gmail.com", null },
                    { 2, "Nice project", "mail@gmail.com", "Dmytro" }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_link_categories",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "book" },
                    { 2, "video" },
                    { 3, "article" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreateDate", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "Index", "Teaser", "Title", "ViewCount", "streetcode_type" },
                values: new object[] { 4, new DateTime(2022, 12, 12, 20, 28, 48, 546, DateTimeKind.Local).AddTicks(404), new DateTime(2022, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Звільнення Херсона (11 листопада 2022) — відвоювання Збройними силами України (ЗСУ) міста Херсона та інших районів Херсонської області та частини Миколаївської області на правому березі Дніпра, тоді як збройні сили РФ Сили відійшли на лівий берег (відомий як відхід росіян з Херсона, 9–11 листопада 2022 р.).", "Звільнення Херсона", 1000, "streetcode_event" });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreateDate", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "FirstName", "Index", "LastName", "MiddleName", "Teaser", "streetcode_type" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 20, 28, 48, 546, DateTimeKind.Local).AddTicks(307), new DateTime(1861, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1814, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Тарас", 1, "Шевченко", "Григорович", "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія, Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. Академік Імператорської академії мистецтв", "streetcode_person" },
                    { 2, new DateTime(2022, 12, 12, 20, 28, 48, 546, DateTimeKind.Local).AddTicks(376), new DateTime(1885, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1817, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Мико́ла", 2, "Костома́ров", "Іва́нович", "Мико́ла Іва́нович Костома́ров (4 (16) травня 1817, с. Юрасівка, Острогозький повіт, Воронезька губернія — 7 (19) квітня 1885, Петербург) — видатний український[8][9][10][11][12] історик, етнограф, прозаїк, поет-романтик, мислитель, громадський діяч, етнопсихолог[13][14][15]. \r\n\r\nБув співзасновником та активним учасником слов'янофільсько-українського київського об'єднання «Кирило - Мефодіївське братство». У 1847 році за участь в українофільському братстві Костомарова арештовують та перевозять з Києва до Петербурга,де він і провів решту свого життя.", "streetcode_person" },
                    { 3, new DateTime(2022, 12, 12, 20, 28, 48, 546, DateTimeKind.Local).AddTicks(380), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Василь", 3, "Білозерський", "Михайлович", "Білозерський Василь Михайлович (1825, хутір Мотронівка, Чернігівщина — 20 лютого (4 березня) 1899) — український громадсько-політичний і культурний діяч, журналіст.", "streetcode_person" }
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
                    { 4, "wictory" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "terms",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Етнографія — суспільствознавча наука, об'єктом дослідження якої є народи, їхня культура і побут, походження, розселення, процеси культурно-побутових відносин на всіх етапах історії людства.", "етнограф" },
                    { 2, "Гра́фіка — вид образотворчого мистецтва, для якого характерна перевага ліній і штрихів, використання контрастів білого та чорного та менше, ніж у живописі, використання кольору. Твори можуть мати як монохромну, так і поліхромну гаму.", "гравер" },
                    { 3, "Кріпа́цтво, або кріпосне́ право, у вузькому сенсі — правова система, або система правових норм при феодалізмі, яка встановлювала залежність селянина від феодала й неповну власність феодала на селянина.", "кріпак" }
                });

            migrationBuilder.InsertData(
                schema: "timeline",
                table: "timeline_items",
                columns: new[] { "Id", "Date", "Description", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(1831, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 2, new DateTime(1830, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Засвідчивши свою відпускну в петербурзькій Палаті цивільного суду, Шевченко став учнем Академії мистецтв, де його наставником став К. Брюллов. За словами Шевченка: «настала найсвітліша доба його життя, незабутні, золоті дні» навчання в Академії мистецтв, яким він присвятив у 1856 році автобіографічну повість «Художник».", "Учень Петербурзької академії мистецтв" }
                });

            migrationBuilder.InsertData(
                schema: "toponyms",
                table: "toponyms",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, null, "вулиця Шевченка" },
                    { 2, null, "парк Шевченка" },
                    { 3, null, "місто Херсон" }
                });

            migrationBuilder.InsertData(
                schema: "media",
                table: "arts",
                columns: new[] { "Id", "Description", "ImageId" },
                values: new object[,]
                {
                    { 1, "«Погруддя жінки» — портрет роботи Тараса Шевченка (копія з невідомого оригіналу) виконаний ним у Вільно в 1830 році на папері італійським олівцем. Розмір 47,5 × 38. Зустрічається також під назвою «Жіноча голівка»", 2 },
                    { 2, "«Погруддя жінки» — портрет роботи Тараса Шевченка (копія з невідомого оригіналу) виконаний ним у Вільно в 1830 році на папері італійським олівцем. Розмір 47,5 × 38. Зустрічається також під назвою «Жіноча голівка»", 3 },
                    { 3, null, 4 }
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
                columns: new[] { "Id", "Latitude", "Longtitude", "StreetcodeId", "coordinate_type" },
                values: new object[,]
                {
                    { 6, 49.8429m, 24.0311m, 1, "coordinate_streetcode" },
                    { 7, 50.4550m, 30.5238m, 2, "coordinate_streetcode" },
                    { 9, 50.4690m, 30.5328m, 3, "coordinate_streetcode" },
                    { 10, 46.3950m, 32.3738m, 4, "coordinate_streetcode" }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "coordinates",
                columns: new[] { "Id", "Latitude", "Longtitude", "ToponymId", "coordinate_type" },
                values: new object[,]
                {
                    { 1, 49.8429m, 24.0311m, 1, "coordinate_toponym" },
                    { 2, 50.4500m, 30.5233m, 1, "coordinate_toponym" },
                    { 3, 47.5m, 37.32m, 1, "coordinate_toponym" },
                    { 4, 50.4600m, 30.5243m, 2, "coordinate_toponym" },
                    { 5, 50.4550m, 30.5238m, 2, "coordinate_toponym" },
                    { 8, 46.3950m, 32.3738m, 3, "coordinate_toponym" }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "facts",
                columns: new[] { "Id", "FactContent", "ImageId", "Title" },
                values: new object[] { 2, " Ознайомившись випадково з рукописними творами Шевченка й вражений ними, П. Мартос виявив до них великий інтерес. Він порадився із Є. Гребінкою і запропонував Шевченку видати їх окремою книжкою, яку згодом назвали «Кобзарем».", 5, "Перший Кобзар" });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "partner_source_links",
                columns: new[] { "Id", "LogoUrl", "PartnerId", "TargetUrl", "Title" },
                values: new object[,]
                {
                    { 1, "", 1, "https://www.linkedin.com/company/softserve/", "LinkedIn" },
                    { 2, "", 1, "https://www.instagram.com/softserve_people/", "Instagram" },
                    { 3, "", 1, "https://www.facebook.com/SoftServeCompany", "facebook" }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_links",
                columns: new[] { "Id", "StreetcodeId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, "Вікіпедія", "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87" },
                    { 2, 1, "Кобзар", "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf" },
                    { 3, 4, "Св'яткування звільнення", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" }
                });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId", "IsSponsor" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 1, 2, true }
                });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId" },
                values: new object[] { 1, 3 });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId", "IsSponsor" },
                values: new object[,]
                {
                    { 1, 4, true },
                    { 2, 1, true }
                });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId", "IsSponsor" },
                values: new object[] { 2, 4, true });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId" },
                values: new object[] { 3, 3 });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "subtitles",
                columns: new[] { "Id", "Description", "FirstName", "LastName", "Status", "StreetcodeId", "Url" },
                values: new object[,]
                {
                    { 1, "description", "Dmytro", "Buchkovsky", (byte)0, 1, "https://t.me/MaisterD" },
                    { 2, "description", "Dmytro", "Buchkovsky", (byte)1, 2, "https://t.me/MaisterD" },
                    { 3, "description", "Dmytro", "Buchkovsky", (byte)0, 3, "https://t.me/MaisterD" },
                    { 4, "description", "Oleksndr", "Lazarenko", (byte)0, 1, null },
                    { 5, null, "Oleksndr", "Lazarenko", (byte)0, 2, null },
                    { 6, null, "Yaroslav", "Chushenko", (byte)1, 1, null },
                    { 7, null, "Yaroslav", "Chushenko", (byte)1, 3, null },
                    { 8, null, "Nazarii", "Hovdysh", (byte)0, 4, null },
                    { 9, null, "Tatiana", "Shumylo", (byte)1, 4, null }
                });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "texts",
                columns: new[] { "Id", "StreetcodeId", "TextContent", "Title" },
                values: new object[] { 1, 1, "Тарас Шевченко народився 9 березня 1814 року в селі Моринці Пединівської волості Звенигородського повіту Київської губернії. Був третьою дитиною селян-кріпаків Григорія Івановича Шевченка та Катерини Якимівни після сестри Катерини (1804 — близько 1848) та брата Микити (1811 — близько 1870).\r\n\r\nЗа родинними переказами, Тарасові діди й прадіди з батьківського боку походили від козака Андрія, який на початку XVIII століття прийшов із Запорізької Січі. Батьки його матері, Катерини Якимівни Бойко, були переселенцями з Прикарпаття.\r\n\r\n1816 року сім'я Шевченків переїхала до села Кирилівка Звенигородського повіту, звідки походив Григорій Іванович. Дитячі роки Тараса пройшли в цьому селі. 1816 року народилася Тарасова сестра Ярина, 1819 року — сестра Марія, а 1821 року народився Тарасів брат Йосип.\r\n\r\nВосени 1822 року Тарас Шевченко почав учитися грамоти у дяка Совгиря. Тоді ж ознайомився з творами Григорія Сковороди.\r\n\r\n10 лютого 1823 року його старша сестра Катерина вийшла заміж за Антона Красицького — селянина із Зеленої Діброви, а 1 вересня 1823 року від тяжкої праці й злиднів померла мати Катерина. \r\n\r\n19 жовтня 1823 року батько одружився вдруге з удовою Оксаною Терещенко, в якої вже було троє дітей. Вона жорстоко поводилася з нерідними дітьми, зокрема з малим Тарасом. 1824 року народилася Тарасова сестра Марія — від другого батькового шлюбу.", "Дитинство та юність" });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "texts",
                columns: new[] { "Id", "StreetcodeId", "TextContent", "Title" },
                values: new object[,]
                {
                    { 2, 2, "Батьки М. І. Костомарова намагалися прищепити сину вільнолюбні ідеї і дати добру освіту. Тому вже з 10 років М. Костомарова відправили навчатися до Московського пансіону, а згодом до Воронезької гімназії, яку той закінчив 1833 р.\r\n\r\n1833 р. М. І. Костомаров вступає на історико-філологічний факультет Харківського університету. Вже у цьому навчальному закладі він проявив непересічні здібності до навчання.\r\n\r\nВ університеті Микола Костомаров вивчав стародавні й нові мови, цікавився античною історією, німецькою філософією і новою французькою літературою, учився грати на фортепіано, пробував писати вірші. Зближення з гуртком українських романтиків Харківського університету незабаром визначило його захоплення переважно фольклором і козацьким минулим України.\r\n\r\nУ ті роки у Харківському університеті навколо професора-славіста і літератора-романтика І. Срезневського сформувався гурток студентів, захоплених збиранням зразків української народної пісенної творчості. Вони сприймали фольклор як вираження народного духу, самі складали вірші, балади і ліричні пісні, звертаючись до народної творчості.\r\n\r\nКостомаров в університетські роки дуже багато читав. Перевантаження позначилося на його здоров'ї — ще за студентства значно погіршився зір.\r\n\r\nНа світогляд М. І. Костомарова вплинули професор грецької літератури Харківського університету А. О. Валицький та професор всесвітньої історії М. М. Лунін.\r\n\r\n1836 р. М. І. Костомаров закінчив університет, а в січні 1837 р. склав іспити на ступінь кандидата й отримав направлення у Кінбурнський 7-й драгунський полк юнкером.\r\n\r\nУ січні 1837 року Костомаров склав іспити з усіх предметів, і 8 грудня 1837 року його затвердили в статусі кандидата.", "Юність і навчання" },
                    { 3, 3, "Народився у дворянській родині на хуторі Мотронівка (нині у межах с. Оленівка поблизу Борзни).\r\n\r\nУ 1843–1846 роках здобув вищу освіту на історико-філологічному факультеті Київського Імператорського університету св. Володимира.\r\n\r\n1846–1847 — учитель Петровського кадетського корпусу у Полтаві.\r\n\r\nРазом з М. Костомаровим і М. Гулаком був організатором Кирило-Мефодіївського братства. Брав участь у створенні «Статуту Слов'янського братства св. Кирила і Мефодія». Автор «Записки» — пояснень до статуту братства. Розвивав ідеї християнського соціалізму, виступав за об'єднання всіх слов'янських народів у республіканську федерацію, в якій провідну роль відводив Україні.\r\n1847 — 10 квітня був заарештований у Варшаві. Засланий до Олонецької губернії під нагляд поліції. Служив у Петрозаводському губернському правлінні.\r\n\r\n1856 — звільнений із заслання. Оселився у Санкт-Петербурзі, де став активним членом місцевого гуртка українців.\r\n\r\n1861–1862 — редактор першого українського щомісячного журналу «Основа».\r\n\r\nЗгодом служив у Варшаві. Підтримував зв'язки з Галичиною, співпрацював у часописах «Мета» і «Правда».\r\n\r\nОстанні роки життя провів на хуторі Мотронівці.", "Життєпис" },
                    { 4, 4, "Експерти пояснили, що дасть херсонська перемога українським силам\r\n\r\nНа тлі заяв окупантів про відведення військ та сил рф від Херсона та просування ЗСУ на херсонському напрямку українські бійці можуть отримати вогневий контроль над найважливішими дорогами Криму. Більше того, звільнення облцентру переріже постачання зброї для росії.", "визволення Херсона" }
                });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "transaction_links",
                columns: new[] { "Id", "QrCodeUrl", "StreetcodeId", "Url" },
                values: new object[,]
                {
                    { 1, "https://qrcode/1", 1, "https://streetcode/1" },
                    { 2, "https://qrcode/2", 2, "https://streetcode/2" },
                    { 3, "https://qrcode/3", 3, "https://streetcode/3" },
                    { 4, "https://qrcode/4", 4, "https://streetcode/4" }
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
                column: "StreetcodeId",
                unique: true,
                filter: "[StreetcodeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_coordinates_ToponymId",
                schema: "add_content",
                table: "coordinates",
                column: "ToponymId");

            migrationBuilder.CreateIndex(
                name: "IX_facts_ImageId",
                schema: "streetcode",
                table: "facts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_partner_source_links_PartnerId",
                schema: "partner_sponsors",
                table: "partner_source_links",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_related_figures_TargetId",
                schema: "streetcode",
                table: "related_figures",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_source_links_StreetcodeId",
                schema: "sources",
                table: "source_links",
                column: "StreetcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceLinkSourceLinkCategory_SourceLinksId",
                schema: "sources",
                table: "SourceLinkSourceLinkCategory",
                column: "SourceLinksId");

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_arts_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_arts",
                column: "StreetcodesId");

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
                name: "IX_streetcode_partners_StreetcodeId",
                schema: "partner_sponsors",
                table: "streetcode_partners",
                column: "StreetcodeId");

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
                name: "partner_source_links",
                schema: "partner_sponsors");

            migrationBuilder.DropTable(
                name: "related_figures",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "responses",
                schema: "feedback");

            migrationBuilder.DropTable(
                name: "SourceLinkSourceLinkCategory",
                schema: "sources");

            migrationBuilder.DropTable(
                name: "streetcode_arts",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_fact",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_image",
                schema: "streetcode");

            migrationBuilder.DropTable(
                name: "streetcode_partners",
                schema: "partner_sponsors");

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
                name: "source_link_categories",
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
                schema: "partner_sponsors");

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
                name: "images",
                schema: "media");
        }
    }
}
