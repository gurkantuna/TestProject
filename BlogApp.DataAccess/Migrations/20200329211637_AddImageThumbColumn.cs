using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogApp.DataAccess.Migrations
{
    public partial class AddImageThumbColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumb",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumb",
                table: "Blogs");
        }
    }
}
