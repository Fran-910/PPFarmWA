using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPFarmWA.BD.Migrations
{
    /// <inheritdoc />
    public partial class PermitirCompradorNulo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "idJugadorComprador",
                table: "Ventas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "idJugadorComprador",
                table: "Ventas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
