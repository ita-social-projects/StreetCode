using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class StreetcodeTypeaddedtostreetcodetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "streetcode",
                table: "streetcodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3762));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3673), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3710), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3716), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3721), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3733), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Type" },
                values: new object[] { new DateTime(2023, 2, 24, 15, 52, 16, 356, DateTimeKind.Local).AddTicks(3738), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(2022));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1942));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1983));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1987));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1991));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1999));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(2003));
        }
    }
}
