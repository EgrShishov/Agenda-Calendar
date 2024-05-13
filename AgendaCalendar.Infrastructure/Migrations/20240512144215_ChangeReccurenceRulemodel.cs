using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaCalendar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReccurenceRulemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReccurenceRules_DaysOfMonth",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_DaysOfWeek",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_Frequency",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_MonthsOfYear",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_RecurrenceDates_Capacity",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_WeeksOfMonth",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_Year",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "ReccurenceRules_Interval",
                table: "Events",
                newName: "ReccurenceRules_interval");

            migrationBuilder.AddColumn<List<string>>(
                name: "ReccurenceRules_byweekday",
                table: "Events",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReccurenceRules_dtstart",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReccurenceRules_freq",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReccurenceRules_until",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReccurenceRules_byweekday",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_dtstart",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_freq",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReccurenceRules_until",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "ReccurenceRules_interval",
                table: "Events",
                newName: "ReccurenceRules_Interval");

            migrationBuilder.AddColumn<List<int>>(
                name: "ReccurenceRules_DaysOfMonth",
                table: "Events",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int[]>(
                name: "ReccurenceRules_DaysOfWeek",
                table: "Events",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int>(
                name: "ReccurenceRules_Frequency",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<int>>(
                name: "ReccurenceRules_MonthsOfYear",
                table: "Events",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "ReccurenceRules_RecurrenceDates_Capacity",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<int>>(
                name: "ReccurenceRules_WeeksOfMonth",
                table: "Events",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "ReccurenceRules_Year",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
