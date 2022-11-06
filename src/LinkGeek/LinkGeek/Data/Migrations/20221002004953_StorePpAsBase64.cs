using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class StorePpAsBase64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureContentType",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureData",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureContentType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureData",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(2048)",
                nullable: true);
        }
    }
}
