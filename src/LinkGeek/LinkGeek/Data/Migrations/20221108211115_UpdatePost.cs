using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class UpdatePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LookingFor",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LookingFor",
                table: "Posts",
                type: "nvarchar(2048)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
