using Microsoft.EntityFrameworkCore.Migrations;

namespace WebPortal.Migrations
{
    public partial class AddedMileage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WillCall",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "Reservations",
                newName: "Mileage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Reservations",
                newName: "Distance");

            migrationBuilder.AddColumn<bool>(
                name: "WillCall",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
