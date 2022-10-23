using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisciplinarySystem.Persistence.Migrations
{
    public partial class RemoveUserRoleTable : Migration
    {
        protected override void Up ( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.AddColumn<long>(
                name: "RoleId" ,
                table: "AuthUser" ,
                type: "bigint" ,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_RoleId" ,
                table: "AuthUser" ,
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthUser_AuthRole_RoleId" ,
                table: "AuthUser" ,
                column: "RoleId" ,
                principalTable: "AuthRole" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down ( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthUser_AuthRole_RoleId" ,
                table: "AuthUser");

            migrationBuilder.DropIndex(
                name: "IX_AuthUser_RoleId" ,
                table: "AuthUser");

            migrationBuilder.DropColumn(
                name: "RoleId" ,
                table: "AuthUser");

            migrationBuilder.CreateTable(
                name: "UserRoles" ,
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint" , nullable: false) ,
                    UserId = table.Column<long>(type: "bigint" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles" , x => new { x.RoleId , x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AuthRole_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AuthRole" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_AuthUser_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AuthUser" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId" ,
                table: "UserRoles" ,
                column: "UserId");
        }
    }
}
