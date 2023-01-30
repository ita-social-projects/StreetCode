using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class ChangeArts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 18,
                column: "Url",
                value: "/assets/2296e9b1db2ab72f2db9.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 19,
                column: "Url",
                value: "/assets/35b44f042d027c3a7589.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 20,
                column: "Url",
                value: "/assets/c58dac51751395fb3217.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 21,
                column: "Url",
                value: "/assets/233c6bbb0b79df230d93.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 22,
                column: "Url",
                value: "/assets/02b59f4ef917107514e3.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 23,
                column: "Url",
                value: "/assets/8ecaa9756bac938f8f73.png");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 2, 1 },
                column: "Index",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 3, 1 },
                column: "Index",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 4, 1 },
                column: "Index",
                value: 4);

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeId", "Index" },
                values: new object[,]
                {
                    { 5, 1, 5 },
                    { 6, 1, 6 }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2353));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2109));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2217));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2226));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2265));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2273));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 30, 19, 30, 37, 34, DateTimeKind.Local).AddTicks(2281));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 18,
                column: "Url",
                value: "/assets/b647ab7ccc32fdb15536.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 19,
                column: "Url",
                value: "/assets/46a4e32fed29974d6562.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 20,
                column: "Url",
                value: "/assets/affcff354ffdf1c788e5.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 21,
                column: "Url",
                value: "/assets/841a6ad9c34f36476c04.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 22,
                column: "Url",
                value: "/assets/05b6e5a005600349283a.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 23,
                column: "Url",
                value: "/assets/4153f1a052db8b5bfdd9.png");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 2, 1 },
                column: "Index",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 3, 1 },
                column: "Index",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcode_art",
                keyColumns: new[] { "ArtId", "StreetcodeId" },
                keyValues: new object[] { 4, 1 },
                column: "Index",
                value: 5);

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcode_art",
                columns: new[] { "ArtId", "StreetcodeId", "Index" },
                values: new object[,]
                {
                    { 2, 2, 7 },
                    { 2, 3, 6 },
                    { 3, 3, 2 }
                });

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
    }
}
