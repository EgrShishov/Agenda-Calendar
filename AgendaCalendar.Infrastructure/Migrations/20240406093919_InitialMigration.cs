using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AgendaCalendar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CalendarDescription = table.Column<string>(type: "text", nullable: false),
                    Subscribers = table.Column<List<int>>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    BirthdayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    EventParticipants_Capacity = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    ReccurenceRules_Frequency = table.Column<int>(type: "integer", nullable: false),
                    ReccurenceRules_Interval = table.Column<int>(type: "integer", nullable: false),
                    ReccurenceRules_DaysOfWeek = table.Column<int[]>(type: "integer[]", nullable: false),
                    ReccurenceRules_DaysOfMonth = table.Column<List<int>>(type: "integer[]", nullable: false),
                    ReccurenceRules_WeeksOfMonth = table.Column<List<int>>(type: "integer[]", nullable: false),
                    ReccurenceRules_MonthsOfYear = table.Column<List<int>>(type: "integer[]", nullable: false),
                    ReccurenceRules_Year = table.Column<int>(type: "integer", nullable: false),
                    ReccurenceRules_RecurrenceDates_Capacity = table.Column<int>(type: "integer", nullable: false),
                    CalendarId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReminderTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    NotificationInterval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CalendarId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CalendarId",
                table: "Events",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_CalendarId",
                table: "Reminders",
                column: "CalendarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Calendars");
        }
    }
}
