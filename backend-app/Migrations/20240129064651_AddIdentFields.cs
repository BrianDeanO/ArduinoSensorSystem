using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEndApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SensorIdent",
                table: "Sensors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceIdent",
                table: "Devices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SensorIdent",
                table: "Sensors",
                column: "SensorIdent",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceIdent",
                table: "Devices",
                column: "DeviceIdent",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensors_SensorIdent",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceIdent",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SensorIdent",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DeviceIdent",
                table: "Devices");
        }
    }
}
