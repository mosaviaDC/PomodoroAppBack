using Microsoft.EntityFrameworkCore.Migrations;

namespace PomodoroApp.Migrations
{
    public partial class pomodoro3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastPeriodTime",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPeriodTime",
                table: "Tasks");
        }
    }
}
