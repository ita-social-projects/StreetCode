using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddQrCoordinates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "coordinates");

            migrationBuilder.CreateTable(
                name: "qr_coordinates",
                schema: "coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoordinateId = table.Column<int>(type: "int", nullable: false),
                    QrId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qr_coordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_qr_coordinates_coordinates_CoordinateId",
                        column: x => x.CoordinateId,
                        principalSchema: "add_content",
                        principalTable: "coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qr_coordinates_CoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                column: "CoordinateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qr_coordinates",
                schema: "coordinates");
        }
    }
}
