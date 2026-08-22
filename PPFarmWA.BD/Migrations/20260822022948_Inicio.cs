using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPFarmWA.BD.Migrations
{
    /// <inheritdoc />
    public partial class Inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    idJugador = table.Column<int>(type: "int", nullable: false),
                    idRecurso = table.Column<int>(type: "int", nullable: false),
                    idVenta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ppCoins = table.Column<double>(type: "float", nullable: false),
                    points = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    experiencia = table.Column<int>(type: "int", nullable: false),
                    esTienda = table.Column<bool>(type: "bit", nullable: false),
                    esAdmin = table.Column<bool>(type: "bit", nullable: false),
                    idUltimaHerramienta = table.Column<int>(type: "int", nullable: false),
                    idUltimoDispositivo = table.Column<int>(type: "int", nullable: false),
                    idUltimoPotenciador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    eficiencia = table.Column<int>(type: "int", nullable: false),
                    durabilidad = table.Column<int>(type: "int", nullable: false),
                    valor = table.Column<double>(type: "float", nullable: false),
                    deTienda = table.Column<bool>(type: "bit", nullable: false),
                    tipo = table.Column<int>(type: "int", nullable: false),
                    idRareza = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idJugadorVendedor = table.Column<int>(type: "int", nullable: false),
                    idJugadorComprador = table.Column<int>(type: "int", nullable: false),
                    cantidadVenta = table.Column<int>(type: "int", nullable: false),
                    precioVenta = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Ventas");
        }
    }
}
