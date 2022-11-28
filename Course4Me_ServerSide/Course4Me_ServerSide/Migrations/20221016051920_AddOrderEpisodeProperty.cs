using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course4Me_ServerSide.Migrations
{
    public partial class AddOrderEpisodeProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderOfEpisode",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderOfEpisode",
                table: "Videos");
        }
    }
}
