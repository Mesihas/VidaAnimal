using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VidaAnimal.Migrations
{
    public partial class SalesDetailMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "TblSales");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TblSales");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "TblSales");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TblSales");

            migrationBuilder.CreateTable(
                name: "TblSalesDetail",
                columns: table => new
                {
                    SalesDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SalesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSalesDetail", x => x.SalesDetailId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblSalesDetail");

            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "TblSales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "TblSales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "TblSales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TblSales",
                nullable: false,
                defaultValue: 0);
        }
    }
}
