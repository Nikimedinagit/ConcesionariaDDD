using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CamposLocalidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocalidadId",
                table: "Empresas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_LocalidadId",
                table: "Empresas",
                column: "LocalidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Localidades_LocalidadId",
                table: "Empresas",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Localidades_LocalidadId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_LocalidadId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "LocalidadId",
                table: "Empresas");
        }
    }
}
