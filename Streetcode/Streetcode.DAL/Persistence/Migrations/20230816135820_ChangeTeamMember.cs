using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class ChangeTeamMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "team",
                table: "team_members");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "team",
                table: "team_members",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "team",
                table: "team_members",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "team",
                table: "team_members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
