using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.DataAccess.Migrations
{
    public partial class ShowInHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInHome",
                table: "Blogs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInHome",
                table: "Blogs");
        }
    }
}
