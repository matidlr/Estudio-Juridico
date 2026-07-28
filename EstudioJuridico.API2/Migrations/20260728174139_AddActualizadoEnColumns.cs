using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstudioJuridico.API2.Migrations
{
    /// <inheritdoc />
    public partial class AddActualizadoEnColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Casos",
                newName: "Proceso");

            migrationBuilder.RenameColumn(
                name: "NombrePartes",
                table: "Casos",
                newName: "NroExpediente");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Casos",
                newName: "Juzgado");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Recordatorios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Recordatorios",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Recordatorios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Pruebas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Pruebas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SeccionExpedienteId",
                table: "Pruebas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Comentarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Comentarios",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Leida",
                table: "Comentarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TipoAutor",
                table: "Comentarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Clientes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Clientes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Casos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caratula",
                table: "Casos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Casos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Archivos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Archivos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SeccionExpedienteId",
                table: "Archivos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AclaracionCliente",
                table: "Actualizaciones",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Actualizaciones",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Actualizaciones",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SeccionExpedienteId",
                table: "Actualizaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Abogados",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreadoEn",
                table: "Abogados",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Entidad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntidadId = table.Column<int>(type: "int", nullable: true),
                    Detalle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultasPublicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensaje = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Atendida = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AreaInteres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultasPublicas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Concepto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Monto = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FormaPago = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CasoId = table.Column<int>(type: "int", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimientos_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermisosCausa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CasoId = table.Column<int>(type: "int", nullable: false),
                    AbogadoId = table.Column<int>(type: "int", nullable: false),
                    OtorgadoPorId = table.Column<int>(type: "int", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosCausa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermisosCausa_Abogados_AbogadoId",
                        column: x => x.AbogadoId,
                        principalTable: "Abogados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermisosCausa_Abogados_OtorgadoPorId",
                        column: x => x.OtorgadoPorId,
                        principalTable: "Abogados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermisosCausa_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Secciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FojaDesde = table.Column<int>(type: "int", nullable: false),
                    FojaHasta = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    CasoId = table.Column<int>(type: "int", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Secciones_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VersionesFoja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActualizacionId = table.Column<int>(type: "int", nullable: false),
                    Contenido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NroFoja = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AclaracionCliente = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ModificadoPorId = table.Column<int>(type: "int", nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionesFoja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersionesFoja_Actualizaciones_ActualizacionId",
                        column: x => x.ActualizacionId,
                        principalTable: "Actualizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionesFoja_Usuarios_ModificadoPorId",
                        column: x => x.ModificadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_SeccionExpedienteId",
                table: "Pruebas",
                column: "SeccionExpedienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_SeccionExpedienteId",
                table: "Archivos",
                column: "SeccionExpedienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Actualizaciones_SeccionExpedienteId",
                table: "Actualizaciones",
                column: "SeccionExpedienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UsuarioId",
                table: "AuditLogs",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_CasoId",
                table: "Movimientos",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosCausa_AbogadoId",
                table: "PermisosCausa",
                column: "AbogadoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosCausa_CasoId",
                table: "PermisosCausa",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosCausa_OtorgadoPorId",
                table: "PermisosCausa",
                column: "OtorgadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_CasoId",
                table: "Secciones",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionesFoja_ActualizacionId",
                table: "VersionesFoja",
                column: "ActualizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionesFoja_ModificadoPorId",
                table: "VersionesFoja",
                column: "ModificadoPorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actualizaciones_Secciones_SeccionExpedienteId",
                table: "Actualizaciones",
                column: "SeccionExpedienteId",
                principalTable: "Secciones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Archivos_Secciones_SeccionExpedienteId",
                table: "Archivos",
                column: "SeccionExpedienteId",
                principalTable: "Secciones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pruebas_Secciones_SeccionExpedienteId",
                table: "Pruebas",
                column: "SeccionExpedienteId",
                principalTable: "Secciones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actualizaciones_Secciones_SeccionExpedienteId",
                table: "Actualizaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Archivos_Secciones_SeccionExpedienteId",
                table: "Archivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pruebas_Secciones_SeccionExpedienteId",
                table: "Pruebas");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ConsultasPublicas");

            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "PermisosCausa");

            migrationBuilder.DropTable(
                name: "Secciones");

            migrationBuilder.DropTable(
                name: "VersionesFoja");

            migrationBuilder.DropIndex(
                name: "IX_Pruebas_SeccionExpedienteId",
                table: "Pruebas");

            migrationBuilder.DropIndex(
                name: "IX_Archivos_SeccionExpedienteId",
                table: "Archivos");

            migrationBuilder.DropIndex(
                name: "IX_Actualizaciones_SeccionExpedienteId",
                table: "Actualizaciones");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Recordatorios");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Recordatorios");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Recordatorios");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Pruebas");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Pruebas");

            migrationBuilder.DropColumn(
                name: "SeccionExpedienteId",
                table: "Pruebas");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "Leida",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "TipoAutor",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Caratula",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Archivos");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Archivos");

            migrationBuilder.DropColumn(
                name: "SeccionExpedienteId",
                table: "Archivos");

            migrationBuilder.DropColumn(
                name: "AclaracionCliente",
                table: "Actualizaciones");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Actualizaciones");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Actualizaciones");

            migrationBuilder.DropColumn(
                name: "SeccionExpedienteId",
                table: "Actualizaciones");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Abogados");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Abogados");

            migrationBuilder.RenameColumn(
                name: "Proceso",
                table: "Casos",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "NroExpediente",
                table: "Casos",
                newName: "NombrePartes");

            migrationBuilder.RenameColumn(
                name: "Juzgado",
                table: "Casos",
                newName: "Descripcion");
        }
    }
}
