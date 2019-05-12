using Microsoft.EntityFrameworkCore.Migrations;

namespace VacationCalendar.Api.Migrations
{
    public partial class addUserOfficeCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfficeCountryCode",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeCountryCode",
                table: "Users");
        }
    }
}
