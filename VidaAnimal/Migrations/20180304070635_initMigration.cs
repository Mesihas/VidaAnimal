using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VidaAnimal.Migrations
{
    public partial class initMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblProduct",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    IVA = table.Column<double>(nullable: false),
                    Marked = table.Column<double>(nullable: false),
                    PriceLastUdate = table.Column<DateTime>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    StockControl = table.Column<int>(nullable: false),
                    StockMinim = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    Units = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblProduct", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "TblPurchasing",
                columns: table => new
                {
                    PurchasingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Obs = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPurchasing", x => x.PurchasingId);
                });

            migrationBuilder.CreateTable(
                name: "TblSales",
                columns: table => new
                {
                    SalesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<int>(nullable: false),
                    Obs = table.Column<string>(nullable: true),
                    PayWayId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SellingDate = table.Column<DateTime>(nullable: false),
                    Total = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSales", x => x.SalesId);
                });

            migrationBuilder.CreateTable(
                name: "TblStock",
                columns: table => new
                {
                    StockId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentStock = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    StockControl = table.Column<int>(nullable: false),
                    StockMinim = table.Column<int>(nullable: false),
                    Unidades = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStock", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "TblSupplier",
                columns: table => new
                {
                    SupplierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    SupplierName = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblSupplier", x => x.SupplierId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblProduct");

            migrationBuilder.DropTable(
                name: "TblPurchasing");

            migrationBuilder.DropTable(
                name: "TblSales");

            migrationBuilder.DropTable(
                name: "TblStock");

            migrationBuilder.DropTable(
                name: "TblSupplier");
        }
    }
}
