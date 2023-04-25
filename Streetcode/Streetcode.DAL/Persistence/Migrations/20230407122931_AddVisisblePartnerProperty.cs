using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddVisisblePartnerProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Teaser",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(650)",
                maxLength: 650,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(520)",
                oldMaxLength: 520);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleEverywhere",
                schema: "partners",
                table: "partners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9284));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9129));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9207));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9214));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9221));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9227));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 7, 15, 29, 29, 815, DateTimeKind.Local).AddTicks(9240));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisibleEverywhere",
                schema: "partners",
                table: "partners");

            migrationBuilder.AlterColumn<string>(
                name: "Teaser",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(520)",
                maxLength: 520,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(650)",
                oldMaxLength: 650);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7460));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7343));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7399));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7407));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7413));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7421));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 4, 4, 11, 40, 40, 299, DateTimeKind.Local).AddTicks(7427));
        }
    }
}
