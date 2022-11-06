using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class AddGameSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Game",
                type: "nvarchar(4096)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4096)");

            migrationBuilder.CreateTable(
                name: "GameSearchCache",
                columns: table => new
                {
                    Query = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSearchCache", x => new { x.Query, x.Rank });
                    table.ForeignKey(
                        name: "FK_GameSearchCache_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSearchCache_GameId",
                table: "GameSearchCache",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSearchCache");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Game",
                type: "nvarchar(4096)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4096)",
                oldNullable: true);
        }
    }
}
