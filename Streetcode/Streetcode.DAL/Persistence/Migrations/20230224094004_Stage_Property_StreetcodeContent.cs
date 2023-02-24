using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class Stage_Property_StreetcodeContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stage",
                schema: "streetcode",
                table: "streetcodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                column: "Url",
                value: "https://i.ibb.co/RB9KtSq/Ukrainka.png");

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
                columns: new[] { "CreatedAt", "Stage" },
                values: new object[] { new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1991), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Stage" },
                values: new object[] { new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(1999), 1 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Stage" },
                values: new object[] { new DateTime(2023, 2, 24, 11, 40, 3, 825, DateTimeKind.Local).AddTicks(2003), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stage",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                column: "Url",
                value: "https://i.ibb.co/f85t1Vs/Antonovich.png");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6954));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6882));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6917));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6922));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6927));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6931));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6935));
        }
    }
}
