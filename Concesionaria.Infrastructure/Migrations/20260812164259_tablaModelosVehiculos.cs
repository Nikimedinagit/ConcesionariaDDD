using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablaModelosVehiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelosVehiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarcaVehiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoVehiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelosVehiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelosVehiculos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelosVehiculos_MarcasVehiculos_MarcaVehiculoId",
                        column: x => x.MarcaVehiculoId,
                        principalTable: "MarcasVehiculos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelosVehiculos_TiposVehiculos_TipoVehiculoId",
                        column: x => x.TipoVehiculoId,
                        principalTable: "TiposVehiculos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelosVehiculos_EmpresaId",
                table: "ModelosVehiculos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelosVehiculos_MarcaVehiculoId",
                table: "ModelosVehiculos",
                column: "MarcaVehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelosVehiculos_TipoVehiculoId",
                table: "ModelosVehiculos",
                column: "TipoVehiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelosVehiculos");
        }
    }
}
