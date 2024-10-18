using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeTeaserRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Teaser",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(650)",
                maxLength: 650,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(650)",
                oldMaxLength: 650,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Teaser",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(650)",
                maxLength: 650,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(650)",
                oldMaxLength: 650);
        }
    }
}
