using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstudioJuridico.API2.Migrations
{
    /// <inheritdoc />
    public partial class AddNroFojaToActualizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NroFoja",
                table: "Actualizaciones",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NroFoja",
                table: "Actualizaciones");
        }
    }
}
