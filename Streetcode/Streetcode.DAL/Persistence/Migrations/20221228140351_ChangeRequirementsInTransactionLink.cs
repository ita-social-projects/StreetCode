using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistance.Migrations
{
    public partial class ChangeRequirementsInTransactionLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "sources",
                table: "source_links",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "sources",
                table: "source_links",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2742));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2566));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2686));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 28, 16, 3, 49, 265, DateTimeKind.Local).AddTicks(2693));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "sources",
                table: "source_links",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "sources",
                table: "source_links",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 23, 22, 21, 11, 540, DateTimeKind.Local).AddTicks(939));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 23, 22, 21, 11, 540, DateTimeKind.Local).AddTicks(843));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 23, 22, 21, 11, 540, DateTimeKind.Local).AddTicks(886));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 23, 22, 21, 11, 540, DateTimeKind.Local).AddTicks(890));
        }
    }
}
