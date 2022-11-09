using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class AddPostFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LookingFor",
                table: "Posts",
                type: "nvarchar(2048)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PlayingAt",
                table: "Posts",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LookingFor",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PlayingAt",
                table: "Posts");
        }
    }
}
