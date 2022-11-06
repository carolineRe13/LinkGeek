﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class MakeGenderOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(4096)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4096)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(4096)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4096)",
                oldNullable: true);
        }
    }
}
