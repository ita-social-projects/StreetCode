using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Migrations
{
    public partial class UpdateToponymEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "toponyms",
                table: "toponyms");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "toponyms",
                table: "toponyms",
                newName: "Oblast");

            migrationBuilder.AddColumn<string>(
                name: "AdminRegionNew",
                schema: "toponyms",
                table: "toponyms",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRegionOld",
                schema: "toponyms",
                table: "toponyms",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Community",
                schema: "toponyms",
                table: "toponyms",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gromada",
                schema: "toponyms",
                table: "toponyms",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                schema: "toponyms",
                table: "toponyms",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 23, 15, 50, 10, 682, DateTimeKind.Local).AddTicks(4843));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 23, 15, 50, 10, 682, DateTimeKind.Local).AddTicks(4781));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 23, 15, 50, 10, 682, DateTimeKind.Local).AddTicks(4823));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 23, 15, 50, 10, 682, DateTimeKind.Local).AddTicks(4828));

            migrationBuilder.UpdateData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Oblast", "StreetName" },
                values: new object[] { "Seed1", "SeedStreet1" });

            migrationBuilder.UpdateData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Oblast", "StreetName" },
                values: new object[] { "Seed2", "SeedStreet2" });

            migrationBuilder.UpdateData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Oblast", "StreetName" },
                values: new object[] { "Seed3", "SeedStreet3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRegionNew",
                schema: "toponyms",
                table: "toponyms");

            migrationBuilder.DropColumn(
                name: "AdminRegionOld",
                schema: "toponyms",
                table: "toponyms");

            migrationBuilder.DropColumn(
                name: "Community",
                schema: "toponyms",
                table: "toponyms");

            migrationBuilder.DropColumn(
                name: "Gromada",
                schema: "toponyms",
                table: "toponyms");

            migrationBuilder.DropColumn(
                name: "StreetName",
                schema: "toponyms",
                table: "toponyms");

            migrationBuilder.RenameColumn(
                name: "Oblast",
                schema: "toponyms",
                table: "toponyms",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "toponyms",
                table: "toponyms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "coordinates",
                columns: new[] { "Id", "CoordinateType", "Latitude", "Longtitude", "ToponymId" },
                values: new object[,]
                {
                    { 1, "coordinate_toponym", 49.8429m, 24.0311m, 1 },
                    { 2, "coordinate_toponym", 50.4500m, 30.5233m, 1 },
                    { 3, "coordinate_toponym", 47.5m, 37.32m, 1 },
                    { 4, "coordinate_toponym", 50.4600m, 30.5243m, 2 },
                    { 5, "coordinate_toponym", 50.4550m, 30.5238m, 2 },
                    { 8, "coordinate_toponym", 46.3950m, 32.3738m, 3 }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(7033));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(6980));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(7016));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 9, 20, 32, 10, 610, DateTimeKind.Local).AddTicks(7019));

            migrationBuilder.UpdateData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title",
                value: "вулиця Шевченка");

            migrationBuilder.UpdateData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "парк Шевченка");

            migrationBuilder.UpdateData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title",
                value: "місто Херсон");
        }
    }
}
