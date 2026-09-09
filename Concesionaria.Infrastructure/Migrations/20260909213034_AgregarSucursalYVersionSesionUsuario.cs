using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSucursalYVersionSesionUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionVersion",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SucursalId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SucursalId",
                table: "AspNetUsers",
                column: "SucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalId",
                table: "AspNetUsers",
                column: "SucursalId",
                principalTable: "Sucursales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SessionVersion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "AspNetUsers");
        }
    }
}
