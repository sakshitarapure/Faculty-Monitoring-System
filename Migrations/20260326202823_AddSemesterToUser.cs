using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacultyMonitoringSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Users");
        }
    }
}
