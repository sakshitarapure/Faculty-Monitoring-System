using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacultyMonitoringSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSemester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimetableEntries_Users_TeacherId",
                table: "TimetableEntries");

            migrationBuilder.DropIndex(
                name: "IX_TimetableEntries_TeacherId",
                table: "TimetableEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_TeacherId",
                table: "TimetableEntries",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimetableEntries_Users_TeacherId",
                table: "TimetableEntries",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
