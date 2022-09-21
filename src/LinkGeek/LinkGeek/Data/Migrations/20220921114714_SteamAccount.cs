
#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkGeek.Migrations
{
    public partial class SteamAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SteamAccount",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamAccount",
                table: "AspNetUsers");
        }
    }
}
