using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class BlobStorageNametoBlobName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlobStorageName",
                schema: "media",
                table: "images",
                newName: "BlobName");

            migrationBuilder.RenameColumn(
                name: "BlobStorageName",
                schema: "media",
                table: "audios",
                newName: "BlobName");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(7053));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(6984));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(7018));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(7022));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(7026));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(7030));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 22, 10, 36, 30, 385, DateTimeKind.Local).AddTicks(7034));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlobName",
                schema: "media",
                table: "images",
                newName: "BlobStorageName");

            migrationBuilder.RenameColumn(
                name: "BlobName",
                schema: "media",
                table: "audios",
                newName: "BlobStorageName");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8483));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8420));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8452));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8456));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8459));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8463));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8466));
        }
    }
}
