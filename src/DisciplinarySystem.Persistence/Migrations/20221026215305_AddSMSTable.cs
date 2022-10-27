using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisciplinarySystem.Persistence.Migrations
{
    public partial class AddSMSTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMS_AuthUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMS_UserId",
                table: "SMS",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMS");
        }
    }
}
