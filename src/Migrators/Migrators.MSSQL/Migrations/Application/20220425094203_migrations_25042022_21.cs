using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class migrations_25042022_21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rate",
                schema: "MPOS",
                table: "Products",
                newName: "SalesPrice");

            migrationBuilder.AddColumn<long>(
                name: "AlertQuantity",
                schema: "MPOS",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                schema: "MPOS",
                table: "Products",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                schema: "MPOS",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CGST",
                schema: "MPOS",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "MPOS",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "MPOS",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HSN",
                schema: "MPOS",
                table: "Products",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "MPOS",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ProfitMargin",
                schema: "MPOS",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                schema: "MPOS",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SGST",
                schema: "MPOS",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                schema: "MPOS",
                table: "Products",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TaxType",
                schema: "MPOS",
                table: "Products",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                schema: "MPOS",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "MPOS",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                schema: "MPOS",
                table: "Products",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "MPOS",
                table: "Products",
                column: "CategoryId",
                principalSchema: "MPOS",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                schema: "MPOS",
                table: "Products",
                column: "UnitId",
                principalSchema: "MPOS",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitId",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AlertQuantity",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Barcode",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CGST",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HSN",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProfitMargin",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SGST",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SKU",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TaxType",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "MPOS",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SalesPrice",
                schema: "MPOS",
                table: "Products",
                newName: "Rate");
        }
    }
}
