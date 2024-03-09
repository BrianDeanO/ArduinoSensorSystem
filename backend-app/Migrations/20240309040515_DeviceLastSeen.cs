using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEndApp.Migrations
{
    /// <inheritdoc />
    public partial class DeviceLastSeen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeviceLastSeen",
                table: "Devices",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceLastSeen",
                table: "Devices");
        }
    }
}
