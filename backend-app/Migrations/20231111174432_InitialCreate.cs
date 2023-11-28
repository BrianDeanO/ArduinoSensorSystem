using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEndApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemperatureItems",
                columns: table => new
                {
                    TemperatureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemperatureValue = table.Column<int>(type: "int", nullable: false),
                    TemperatureUnit = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    TemperatureTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureItems", x => x.TemperatureID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemperatureItems");
        }
    }
}
