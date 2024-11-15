using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class migrations_29042022_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseProducts_Products_ProductId",
                schema: "MPOS",
                table: "PurchaseProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseProducts_ProductId",
                schema: "MPOS",
                table: "PurchaseProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseProducts_ProductId",
                schema: "MPOS",
                table: "PurchaseProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseProducts_Products_ProductId",
                schema: "MPOS",
                table: "PurchaseProducts",
                column: "ProductId",
                principalSchema: "MPOS",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
