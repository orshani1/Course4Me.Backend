using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course4Me_ServerSide.Migrations
{
    public partial class AddCourseIdONVideoObj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Videos");
        }
    }
}
