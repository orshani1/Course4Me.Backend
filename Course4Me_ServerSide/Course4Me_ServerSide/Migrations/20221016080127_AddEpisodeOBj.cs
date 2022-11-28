using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course4Me_ServerSide.Migrations
{
    public partial class AddEpisodeOBj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderOfEpisode",
                table: "Videos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderOfEpisode",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
