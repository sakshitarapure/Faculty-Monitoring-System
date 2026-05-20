using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacultyMonitoringSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentProper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "TimetableEntries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "TimetableEntries");
        }
    }
}
