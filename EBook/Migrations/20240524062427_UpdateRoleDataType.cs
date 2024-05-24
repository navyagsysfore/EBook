using Microsoft.EntityFrameworkCore.Migrations;

namespace EBook.Migrations
{
    public partial class UpdateRoleDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true, // or false, depending on your requirements
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: true, // or false, depending on your requirements
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
