using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class StatisticRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_qr_coordinates_coordinates_CoordinateId",
                schema: "coordinates",
                table: "qr_coordinates");

            migrationBuilder.RenameColumn(
                name: "CoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                newName: "StreetcodeId");

            migrationBuilder.RenameIndex(
                name: "IX_qr_coordinates_CoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                newName: "IX_qr_coordinates_StreetcodeId");

            migrationBuilder.AddColumn<int>(
                name: "StreetcodeCoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_qr_coordinates_StreetcodeCoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                column: "StreetcodeCoordinateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_qr_coordinates_coordinates_StreetcodeCoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                column: "StreetcodeCoordinateId",
                principalSchema: "add_content",
                principalTable: "coordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_qr_coordinates_streetcodes_StreetcodeId",
                schema: "coordinates",
                table: "qr_coordinates",
                column: "StreetcodeId",
                principalSchema: "streetcode",
                principalTable: "streetcodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_qr_coordinates_coordinates_StreetcodeCoordinateId",
                schema: "coordinates",
                table: "qr_coordinates");

            migrationBuilder.DropForeignKey(
                name: "FK_qr_coordinates_streetcodes_StreetcodeId",
                schema: "coordinates",
                table: "qr_coordinates");

            migrationBuilder.DropIndex(
                name: "IX_qr_coordinates_StreetcodeCoordinateId",
                schema: "coordinates",
                table: "qr_coordinates");

            migrationBuilder.DropColumn(
                name: "StreetcodeCoordinateId",
                schema: "coordinates",
                table: "qr_coordinates");

            migrationBuilder.RenameColumn(
                name: "StreetcodeId",
                schema: "coordinates",
                table: "qr_coordinates",
                newName: "CoordinateId");

            migrationBuilder.RenameIndex(
                name: "IX_qr_coordinates_StreetcodeId",
                schema: "coordinates",
                table: "qr_coordinates",
                newName: "IX_qr_coordinates_CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_qr_coordinates_coordinates_CoordinateId",
                schema: "coordinates",
                table: "qr_coordinates",
                column: "CoordinateId",
                principalSchema: "add_content",
                principalTable: "coordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
