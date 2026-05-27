using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregueCamposAuditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Cuentas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Cuentas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Cuentas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Cuentas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CategoriasGastos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CategoriasGastos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CategoriasGastos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CategoriasGastos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CategoriasGastos");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CategoriasGastos");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CategoriasGastos");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CategoriasGastos");
        }
    }
}
