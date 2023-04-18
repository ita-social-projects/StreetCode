using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Update.Internal;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class UpdateSubtitleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "add_content",
                table: "subtitles");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "add_content",
                table: "subtitles");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "add_content",
                table: "subtitles");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "add_content",
                table: "subtitles");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "add_content",
                table: "subtitles");

            migrationBuilder.DropColumn(
                name: "Url",
                schema: "add_content",
                table: "subtitles");

            migrationBuilder.AddColumn<string>(
                name: "SubtitleText",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "subtitles",
                columns: new[] { "Id", "SubtitleText", "StreetcodeId" },
                values: new object[,]
                {
                    { 1, "Developers: StreedCodeTeam, some text, some more text, and more text", 4 },
                    { 2, "Developers: StreedCodeTeam, some text, some more text, and more text", 3 },
                    { 3, "Developers: StreedCodeTeam, some text, some more text, and more text", 2 },
                    { 4, "Developers: StreedCodeTeam, some text, some more text, and more text", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
				schema: "add_content",
				table: "subtitles",
				keyColumn: "Id",
				keyValue: 1);

            migrationBuilder.DeleteData(
				schema: "add_content",
				table: "subtitles",
				keyColumn: "Id",
				keyValue: 2);

            migrationBuilder.DeleteData(
				schema: "add_content",
				table: "subtitles",
				keyColumn: "Id",
				keyValue: 3);

            migrationBuilder.DeleteData(
				schema: "add_content",
				table: "subtitles",
				keyColumn: "Id",
				keyValue: 4);

            migrationBuilder.DropColumn(
                 name: "SubtitleText",
                 schema: "add_content",
                 table: "subtitles");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                schema: "add_content",
                table: "subtitles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "add_content",
                table: "subtitles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "subtitles",
                columns: new[] { "Id", "Description", "FirstName", "LastName", "Status", "StreetcodeId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "description", "Dmytro", "Buchkovsky", (byte)0, 1, null, "https://t.me/MaisterD" },
                    { 2, "description", "Dmytro", "Buchkovsky", (byte)1, 2, null, "https://t.me/MaisterD" },
                    { 3, "description", "Dmytro", "Buchkovsky", (byte)0, 3, null, "https://t.me/MaisterD" },
                    { 4, "description", "Oleksndr", "Lazarenko", (byte)0, 1, null, null },
                    { 5, null, "Oleksndr", "Lazarenko", (byte)0, 2, null, null },
                    { 6, null, "Yaroslav", "Chushenko", (byte)1, 1, null, null },
                    { 7, null, "Yaroslav", "Chushenko", (byte)1, 3, null, null },
                    { 8, null, "Nazarii", "Hovdysh", (byte)0, 4, null, null },
                    { 9, null, "Tatiana", "Shumylo", (byte)1, 4, null, null }
                });
        }
    }
}
