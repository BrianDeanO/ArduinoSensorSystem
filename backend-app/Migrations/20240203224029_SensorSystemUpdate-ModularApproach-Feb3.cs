using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEndApp.Migrations
{
    /// <inheritdoc />
    public partial class SensorSystemUpdateModularApproachFeb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceSensors");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "UserLastName");

            migrationBuilder.AddColumn<string>(
                name: "UserFirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DeviceID",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_DeviceID",
                table: "UserDevices",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceID",
                table: "Sensors",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_SensorID",
                table: "SensorDatas",
                column: "SensorID");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorDatas_Sensors_SensorID",
                table: "SensorDatas",
                column: "SensorID",
                principalTable: "Sensors",
                principalColumn: "SensorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Devices_DeviceID",
                table: "Sensors",
                column: "DeviceID",
                principalTable: "Devices",
                principalColumn: "DeviceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Devices_DeviceID",
                table: "UserDevices",
                column: "DeviceID",
                principalTable: "Devices",
                principalColumn: "DeviceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Users_UserID",
                table: "UserDevices",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorDatas_Sensors_SensorID",
                table: "SensorDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Devices_DeviceID",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Devices_DeviceID",
                table: "UserDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Users_UserID",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_UserDevices_DeviceID",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DeviceID",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_SensorDatas_SensorID",
                table: "SensorDatas");

            migrationBuilder.DropColumn(
                name: "UserFirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeviceID",
                table: "Sensors");

            migrationBuilder.RenameColumn(
                name: "UserLastName",
                table: "Users",
                newName: "UserName");

            migrationBuilder.CreateTable(
                name: "DeviceSensors",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    SensorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSensors", x => new { x.DeviceID, x.SensorID });
                });
        }
    }
}
