using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddStatistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "analytics");

            migrationBuilder.CreateTable(
                name: "qr_statistics",
                schema: "analytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoordinateId = table.Column<int>(type: "int", nullable: false),
                    QrId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qr_statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_qr_statistics_coordinates_CoordinateId",
                        column: x => x.CoordinateId,
                        principalSchema: "add_content",
                        principalTable: "coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qr_statistics_CoordinateId",
                schema: "analytics",
                table: "qr_statistics",
                column: "CoordinateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qr_statistics",
                schema: "analytics");
        }
    }
}
