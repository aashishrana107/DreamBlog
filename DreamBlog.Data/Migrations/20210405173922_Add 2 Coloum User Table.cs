using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamBlog.Data.Migrations
{
    public partial class Add2ColoumUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutContent",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubHeader",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutContent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubHeader",
                table: "AspNetUsers");
        }
    }
}
