using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class StoreTableColumnNameRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoreName",
                schema: "MPOS",
                table: "Stores",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StoreCode",
                schema: "MPOS",
                table: "Stores",
                newName: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "MPOS",
                table: "Stores",
                newName: "StoreName");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "MPOS",
                table: "Stores",
                newName: "StoreCode");
        }
    }
}
