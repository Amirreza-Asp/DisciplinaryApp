using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisciplinarySystem.Persistence.Migrations
{
    public partial class AddUserNameAndPasswordToAuthUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AuthUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AuthUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "AuthUser");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AuthUser");
        }
    }
}
