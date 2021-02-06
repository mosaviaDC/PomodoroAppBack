using Microsoft.EntityFrameworkCore.Migrations;

namespace PomodoroApp.Migrations
{
    public partial class tele2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "telegramChatId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "telegramChatId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
