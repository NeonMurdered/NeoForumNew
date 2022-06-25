using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoForum.Migrations
{
    public partial class initnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AdminMessages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "AdminMessages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
