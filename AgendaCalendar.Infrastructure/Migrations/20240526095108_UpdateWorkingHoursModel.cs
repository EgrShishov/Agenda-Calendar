using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaCalendar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkingHoursModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WorkingHours",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkingHours");
        }
    }
}
