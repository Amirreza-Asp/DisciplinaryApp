using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisciplinarySystem.Persistence.Migrations
{
    public partial class AddRelationBetweenUserAndAuthUserTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AuthUser",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_UserId",
                table: "AuthUser",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthUser_Users_UserId",
                table: "AuthUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthUser_Users_UserId",
                table: "AuthUser");

            migrationBuilder.DropIndex(
                name: "IX_AuthUser_UserId",
                table: "AuthUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuthUser");
        }
    }
}
