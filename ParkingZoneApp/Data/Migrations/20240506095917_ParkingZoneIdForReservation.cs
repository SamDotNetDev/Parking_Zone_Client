using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingZoneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ParkingZoneIdForReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingZoneId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingZoneId",
                table: "Reservations");
        }
    }
}
