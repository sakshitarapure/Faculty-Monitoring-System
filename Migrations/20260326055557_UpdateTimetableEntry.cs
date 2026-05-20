using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacultyMonitoringSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimetableEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "TimetableEntries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "TimetableEntries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "TimetableEntries");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "TimetableEntries");
        }
    }
}
