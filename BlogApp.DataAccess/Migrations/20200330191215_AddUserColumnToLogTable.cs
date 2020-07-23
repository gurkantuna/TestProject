using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.DataAccess.Migrations
{
    public partial class AddUserColumnToLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Logs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Logs");
        }
    }
}
