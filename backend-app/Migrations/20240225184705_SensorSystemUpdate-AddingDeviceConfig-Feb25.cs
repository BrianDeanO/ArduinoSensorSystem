using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEndApp.Migrations
{
    /// <inheritdoc />
    public partial class SensorSystemUpdateAddingDeviceConfigFeb25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceIdent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DevicePollingInterval = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserNotifications = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    SensorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorIdent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SensorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelCount = table.Column<int>(type: "int", nullable: false),
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.SensorID);
                    table.ForeignKey(
                        name: "FK_Sensors_Devices_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "DeviceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDevices",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => new { x.UserID, x.DeviceID });
                    table.ForeignKey(
                        name: "FK_UserDevices_Devices_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "DeviceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDevices_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorDatas",
                columns: table => new
                {
                    SensorDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelID = table.Column<int>(type: "int", nullable: false),
                    DataValue = table.Column<float>(type: "real", nullable: false),
                    DataUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeRecorded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SensorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDatas", x => x.SensorDataID);
                    table.ForeignKey(
                        name: "FK_SensorDatas_Sensors_SensorID",
                        column: x => x.SensorID,
                        principalTable: "Sensors",
                        principalColumn: "SensorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceIdent",
                table: "Devices",
                column: "DeviceIdent",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_SensorID",
                table: "SensorDatas",
                column: "SensorID");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceID",
                table: "Sensors",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SensorIdent",
                table: "Sensors",
                column: "SensorIdent",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_DeviceID",
                table: "UserDevices",
                column: "DeviceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorDatas");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
