using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class renameStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_states_Countries_CountryId",
                schema: "MPOS",
                table: "states");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_states_StateId",
                schema: "MPOS",
                table: "Stores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_states",
                schema: "MPOS",
                table: "states");

            migrationBuilder.RenameTable(
                name: "states",
                schema: "MPOS",
                newName: "States",
                newSchema: "MPOS");

            migrationBuilder.RenameIndex(
                name: "IX_states_CountryId",
                schema: "MPOS",
                table: "States",
                newName: "IX_States_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_States",
                schema: "MPOS",
                table: "States",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Countries_CountryId",
                schema: "MPOS",
                table: "States",
                column: "CountryId",
                principalSchema: "MPOS",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_States_StateId",
                schema: "MPOS",
                table: "Stores",
                column: "StateId",
                principalSchema: "MPOS",
                principalTable: "States",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_States_Countries_CountryId",
                schema: "MPOS",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_States_StateId",
                schema: "MPOS",
                table: "Stores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_States",
                schema: "MPOS",
                table: "States");

            migrationBuilder.RenameTable(
                name: "States",
                schema: "MPOS",
                newName: "states",
                newSchema: "MPOS");

            migrationBuilder.RenameIndex(
                name: "IX_States_CountryId",
                schema: "MPOS",
                table: "states",
                newName: "IX_states_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_states",
                schema: "MPOS",
                table: "states",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_states_Countries_CountryId",
                schema: "MPOS",
                table: "states",
                column: "CountryId",
                principalSchema: "MPOS",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_states_StateId",
                schema: "MPOS",
                table: "Stores",
                column: "StateId",
                principalSchema: "MPOS",
                principalTable: "states",
                principalColumn: "Id");
        }
    }
}
