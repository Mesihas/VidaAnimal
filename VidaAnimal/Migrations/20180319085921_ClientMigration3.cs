using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VidaAnimal.Migrations
{
    public partial class ClientMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "TblClient",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "Direccion",
                table: "TblClient",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "TblClient",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "TblClient",
                newName: "Direccion");
        }
    }
}
