using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class RemoveToponymSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9024));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9098));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9107));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9114));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9120));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 21, 14, 8, 355, DateTimeKind.Local).AddTicks(9128));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9329));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9244));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9291));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9295));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9299));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9302));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 29, 20, 57, 44, 923, DateTimeKind.Local).AddTicks(9306));

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
        }
    }
}
