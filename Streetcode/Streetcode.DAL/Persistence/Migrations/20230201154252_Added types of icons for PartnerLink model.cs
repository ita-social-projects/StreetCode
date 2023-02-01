using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddedtypesoficonsforPartnerLinkmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                schema: "partners",
                table: "partner_source_links");

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                schema: "partners",
                table: "partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "LogoType",
                schema: "partners",
                table: "partner_source_links",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TargetUrl", "Title" },
                values: new object[] { "https://twitter.com/SoftServeInc", "Twitter" });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 2,
                column: "LogoType",
                value: (byte)1);

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 3,
                column: "LogoType",
                value: (byte)2);

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlTitle",
                value: "go to SoftServe page");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlTitle",
                schema: "partners",
                table: "partners");

            migrationBuilder.DropColumn(
                name: "LogoType",
                schema: "partners",
                table: "partner_source_links");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "partners",
                table: "partner_source_links",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LogoUrl", "TargetUrl", "Title" },
                values: new object[] { "https://play-lh.googleusercontent.com/kMofEFLjobZy_bCuaiDogzBcUT-dz3BBbOrIEjJ-hqOabjK8ieuevGe6wlTD15QzOqw", "https://www.linkedin.com/company/softserve/", "LinkedIn" });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 2,
                column: "LogoUrl",
                value: "https://www.facebook.com/images/fb_icon_325x325.png");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 3,
                column: "LogoUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/thumb/9/95/Instagram_logo_2022.svg/1200px-Instagram_logo_2022.svg.png");

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
    }
}
