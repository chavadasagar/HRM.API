using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations
{
    public partial class add_estimate_expence_invoice_ProvidentFund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateItems_Designation_AmountId",
                schema: "HRM",
                table: "EstimateItems");

            migrationBuilder.DropColumn(
                name: "ItemName",
                schema: "HRM",
                table: "EstimateItems");

            migrationBuilder.RenameColumn(
                name: "AmountId",
                schema: "HRM",
                table: "EstimateItems",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateItems_AmountId",
                schema: "HRM",
                table: "EstimateItems",
                newName: "IX_EstimateItems_ItemId");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                schema: "HRM",
                table: "Tax",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "HRM",
                table: "EstimateItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FundType",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtherInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "HRM",
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoice_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "HRM",
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoice_Tax_TaxId",
                        column: x => x.TaxId,
                        principalSchema: "HRM",
                        principalTable: "Tax",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxStatus",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvidentFund",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FundTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeShareInAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrganizationShareInAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeShareInPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrganizationShareInPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidentFund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvidentFund_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProvidentFund_FundType_FundTypeId",
                        column: x => x.FundTypeId,
                        principalSchema: "HRM",
                        principalTable: "FundType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "HRM",
                        principalTable: "Invoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "HRM",
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_Employee_PurchaseById",
                        column: x => x.PurchaseById,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expense_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "HRM",
                        principalTable: "Item",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expense_PaymentType_PaidById",
                        column: x => x.PaidById,
                        principalSchema: "HRM",
                        principalTable: "PaymentType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expense_TaxStatus_TaxStatusId",
                        column: x => x.TaxStatusId,
                        principalSchema: "HRM",
                        principalTable: "TaxStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseAttachments",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseAttachments_Expense_ExpenseId",
                        column: x => x.ExpenseId,
                        principalSchema: "HRM",
                        principalTable: "Expense",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tax_StatusId",
                schema: "HRM",
                table: "Tax",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ItemId",
                schema: "HRM",
                table: "Expense",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_PaidById",
                schema: "HRM",
                table: "Expense",
                column: "PaidById");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_PurchaseById",
                schema: "HRM",
                table: "Expense",
                column: "PurchaseById");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_TaxStatusId",
                schema: "HRM",
                table: "Expense",
                column: "TaxStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseAttachments_ExpenseId",
                schema: "HRM",
                table: "ExpenseAttachments",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ClientId",
                schema: "HRM",
                table: "Invoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ProjectId",
                schema: "HRM",
                table: "Invoice",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_TaxId",
                schema: "HRM",
                table: "Invoice",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                schema: "HRM",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ItemId",
                schema: "HRM",
                table: "InvoiceItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidentFund_EmployeeId",
                schema: "HRM",
                table: "ProvidentFund",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvidentFund_FundTypeId",
                schema: "HRM",
                table: "ProvidentFund",
                column: "FundTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateItems_Item_ItemId",
                schema: "HRM",
                table: "EstimateItems",
                column: "ItemId",
                principalSchema: "HRM",
                principalTable: "Item",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tax_TaxStatus_StatusId",
                schema: "HRM",
                table: "Tax",
                column: "StatusId",
                principalSchema: "HRM",
                principalTable: "TaxStatus",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimateItems_Item_ItemId",
                schema: "HRM",
                table: "EstimateItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Tax_TaxStatus_StatusId",
                schema: "HRM",
                table: "Tax");

            migrationBuilder.DropTable(
                name: "ExpenseAttachments",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "InvoiceItems",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "ProvidentFund",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Expense",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Invoice",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "FundType",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "PaymentType",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "TaxStatus",
                schema: "HRM");

            migrationBuilder.DropIndex(
                name: "IX_Tax_StatusId",
                schema: "HRM",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "StatusId",
                schema: "HRM",
                table: "Tax");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "HRM",
                table: "EstimateItems");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "HRM",
                table: "EstimateItems",
                newName: "AmountId");

            migrationBuilder.RenameIndex(
                name: "IX_EstimateItems_ItemId",
                schema: "HRM",
                table: "EstimateItems",
                newName: "IX_EstimateItems_AmountId");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                schema: "HRM",
                table: "EstimateItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateItems_Designation_AmountId",
                schema: "HRM",
                table: "EstimateItems",
                column: "AmountId",
                principalSchema: "HRM",
                principalTable: "Designation",
                principalColumn: "Id");
        }
    }
}
