using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablaProveedoresLocalidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocalidadId",
                table: "Proveedores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_LocalidadId",
                table: "Proveedores",
                column: "LocalidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_Localidades_LocalidadId",
                table: "Proveedores",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_Localidades_LocalidadId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_LocalidadId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "LocalidadId",
                table: "Proveedores");
        }
    }
}
