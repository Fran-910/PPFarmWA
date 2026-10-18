using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPFarmWA.BD.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDurabilidadAItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "durabilidad",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "durabilidad",
                table: "Items");
        }
    }
}
