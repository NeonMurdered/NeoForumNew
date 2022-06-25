using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeoForum.Migrations
{
    public partial class initimgnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "AdminMessages",
                newName: "ProfileImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "AdminMessages",
                newName: "ProfilePicture");
        }
    }
}
