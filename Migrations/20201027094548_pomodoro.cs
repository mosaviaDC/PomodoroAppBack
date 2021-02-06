using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PomodoroApp.Migrations
{
    public partial class pomodoro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskTime",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "CurrentPomodoroPauseTime",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentPomodoroTime",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "InPomodoroPause",
                table: "Tasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaskDateTime",
                table: "Tasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TaskPeriods",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPomodoroPauseTime",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CurrentPomodoroTime",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "InPomodoroPause",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskDateTime",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskPeriods",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskTime",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
