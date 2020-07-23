using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.DataAccess.Migrations
{
    public partial class RemoveShowColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInHome",
                table: "Blogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInHome",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
