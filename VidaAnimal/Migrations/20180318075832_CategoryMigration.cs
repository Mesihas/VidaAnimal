using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VidaAnimal.Migrations
{
    public partial class CategoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Details = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblProduct_CategoryId",
                table: "TblProduct",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblProduct_TblCategory_CategoryId",
                table: "TblProduct",
                column: "CategoryId",
                principalTable: "TblCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblProduct_TblCategory_CategoryId",
                table: "TblProduct");

            migrationBuilder.DropTable(
                name: "TblCategory");

            migrationBuilder.DropIndex(
                name: "IX_TblProduct_CategoryId",
                table: "TblProduct");
        }
    }
}
