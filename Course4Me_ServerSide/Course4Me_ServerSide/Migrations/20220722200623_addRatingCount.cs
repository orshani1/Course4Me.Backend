using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course4Me_ServerSide.Migrations
{
    public partial class addRatingCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Courses");
        }
    }
}
