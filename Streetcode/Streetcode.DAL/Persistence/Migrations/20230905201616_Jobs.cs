using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class Jobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "team",
                table: "team_members");

            migrationBuilder.EnsureSchema(
                name: "jobs");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "team",
                table: "team_members",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "job",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Salary = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job",
                schema: "jobs");

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
