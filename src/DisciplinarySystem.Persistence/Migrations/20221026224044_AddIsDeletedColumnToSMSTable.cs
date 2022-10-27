using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisciplinarySystem.Persistence.Migrations
{
    public partial class AddIsDeletedColumnToSMSTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SMS",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SMS");
        }
    }
}
