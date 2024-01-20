using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations
{
    public partial class add_tiket_and_estimate_module : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tax",
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
                    table.PrimaryKey("PK_Tax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "HRM",
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ticket_Employee_AssigneeId",
                        column: x => x.AssigneeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ticket_Priority_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "HRM",
                        principalTable: "Priority",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Estimate",
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
                    EstimateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Estimate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estimate_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "HRM",
                        principalTable: "Client",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Estimate_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "HRM",
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Estimate_Tax_TaxId",
                        column: x => x.TaxId,
                        principalSchema: "HRM",
                        principalTable: "Tax",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CCStaff",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CCStaff_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CCStaff_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "HRM",
                        principalTable: "Ticket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Followers",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Followers_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Followers_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "HRM",
                        principalTable: "Ticket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketStaff",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketStaff_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketStaff_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "HRM",
                        principalTable: "Ticket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstimateItems",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstimateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    AmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimateItems_Designation_AmountId",
                        column: x => x.AmountId,
                        principalSchema: "HRM",
                        principalTable: "Designation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EstimateItems_Estimate_EstimateId",
                        column: x => x.EstimateId,
                        principalSchema: "HRM",
                        principalTable: "Estimate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CCStaff_EmployeeId",
                schema: "HRM",
                table: "CCStaff",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CCStaff_TicketId",
                schema: "HRM",
                table: "CCStaff",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_ClientId",
                schema: "HRM",
                table: "Estimate",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_ProjectId",
                schema: "HRM",
                table: "Estimate",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_TaxId",
                schema: "HRM",
                table: "Estimate",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateItems_AmountId",
                schema: "HRM",
                table: "EstimateItems",
                column: "AmountId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateItems_EstimateId",
                schema: "HRM",
                table: "EstimateItems",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_EmployeeId",
                schema: "HRM",
                table: "Followers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_TicketId",
                schema: "HRM",
                table: "Followers",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AssigneeId",
                schema: "HRM",
                table: "Ticket",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ClientId",
                schema: "HRM",
                table: "Ticket",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_PriorityId",
                schema: "HRM",
                table: "Ticket",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStaff_EmployeeId",
                schema: "HRM",
                table: "TicketStaff",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStaff_TicketId",
                schema: "HRM",
                table: "TicketStaff",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CCStaff",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "EstimateItems",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Followers",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "TicketStaff",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Estimate",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Ticket",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "Tax",
                schema: "HRM");
        }
    }
}
