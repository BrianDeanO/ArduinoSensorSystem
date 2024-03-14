using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEndApp.Migrations
{
    /// <inheritdoc />
    public partial class SensorSystemUpdateAddingIsDeletedMar11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserIsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SensorIsDeleted",
                table: "Sensors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeviceIsDeleted",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SensorIsDeleted",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DeviceIsDeleted",
                table: "Devices");
        }
    }
}
