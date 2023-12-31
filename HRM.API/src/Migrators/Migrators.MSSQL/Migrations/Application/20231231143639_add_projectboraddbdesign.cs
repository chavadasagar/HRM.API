using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations
{
    public partial class add_projectboraddbdesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                schema: "HRM",
                table: "Employee");

            migrationBuilder.CreateTable(
                name: "LeaveStatus",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskBoard",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskBoard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskBoard_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "HRM",
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskPriority",
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
                    table.PrimaryKey("PK_TaskPriority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leave",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LeaveStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leave_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leave_LeaveStatus_LeaveStatusId",
                        column: x => x.LeaveStatusId,
                        principalSchema: "HRM",
                        principalTable: "LeaveStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leave_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "HRM",
                        principalTable: "LeaveType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskBoard",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskPriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectTaskBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskBoard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskBoard_ProjectTaskBoard_ProjectTaskBoardId",
                        column: x => x.ProjectTaskBoardId,
                        principalSchema: "HRM",
                        principalTable: "ProjectTaskBoard",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskBoard_TaskPriority_TaskPriorityId",
                        column: x => x.TaskPriorityId,
                        principalSchema: "HRM",
                        principalTable: "TaskPriority",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskBoardFollowers",
                schema: "HRM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_TaskBoardFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskBoardFollowers_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HRM",
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskBoardFollowers_TaskBoard_TaskBoardId",
                        column: x => x.TaskBoardId,
                        principalSchema: "HRM",
                        principalTable: "TaskBoard",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leave_EmployeeId",
                schema: "HRM",
                table: "Leave",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_LeaveStatusId",
                schema: "HRM",
                table: "Leave",
                column: "LeaveStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_LeaveTypeId",
                schema: "HRM",
                table: "Leave",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskBoard_ProjectId",
                schema: "HRM",
                table: "ProjectTaskBoard",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBoard_ProjectTaskBoardId",
                schema: "HRM",
                table: "TaskBoard",
                column: "ProjectTaskBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBoard_TaskPriorityId",
                schema: "HRM",
                table: "TaskBoard",
                column: "TaskPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBoardFollowers_EmployeeId",
                schema: "HRM",
                table: "TaskBoardFollowers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBoardFollowers_TaskBoardId",
                schema: "HRM",
                table: "TaskBoardFollowers",
                column: "TaskBoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leave",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "TaskBoardFollowers",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "LeaveStatus",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "LeaveType",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "TaskBoard",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "ProjectTaskBoard",
                schema: "HRM");

            migrationBuilder.DropTable(
                name: "TaskPriority",
                schema: "HRM");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "HRM",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
