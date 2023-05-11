using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class NewSizeTextTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "streetcode",
                table: "texts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TextContent",
                schema: "streetcode",
                table: "texts",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalText",
                schema: "streetcode",
                table: "texts",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "streetcode",
                table: "texts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "TextContent",
                schema: "streetcode",
                table: "texts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalText",
                schema: "streetcode",
                table: "texts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true);
        }
    }
}
