using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TablaCuentaModificada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CuentaPadreId",
                table: "Cuentas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_CuentaPadreId",
                table: "Cuentas",
                column: "CuentaPadreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Cuentas_CuentaPadreId",
                table: "Cuentas",
                column: "CuentaPadreId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Cuentas_CuentaPadreId",
                table: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_Cuentas_CuentaPadreId",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "CuentaPadreId",
                table: "Cuentas");
        }
    }
}
