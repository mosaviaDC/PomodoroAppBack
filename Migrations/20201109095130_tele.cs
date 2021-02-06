using Microsoft.EntityFrameworkCore.Migrations;

namespace PomodoroApp.Migrations
{
    public partial class tele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "telegramChatId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "telegramUserName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "telegramChatId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "telegramUserName",
                table: "AspNetUsers");
        }
    }
}
