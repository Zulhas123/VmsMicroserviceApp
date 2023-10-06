using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Modelmodify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VisitorApprovalLayer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "VisitorApprovalLayer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "VisitorApprovalLayer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "VisitorApprovalLayer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VisitorApprovalLayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VisitorApprovalLayer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "VisitorApprovalLayer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EntityValue",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EntityValue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EntityValue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "EntityValue",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EntityValue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EntityValue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "EntityValue",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EntityType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EntityType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EntityType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "EntityType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EntityType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EntityType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "EntityType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EmployeeData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeConfig",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EmployeeConfig",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeConfig",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "photo",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "signature",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Department",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Department",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CardApprovalLayer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "CardApprovalLayer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CardApprovalLayer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "CardApprovalLayer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CardApprovalLayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CardApprovalLayer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "CardApprovalLayer",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "VisitorApprovalLayer");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EntityValue");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeData");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeData");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeData");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeConfig");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeConfig");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeConfig");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "photo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "signature",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CardApprovalLayer");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CardApprovalLayer");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CardApprovalLayer");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CardApprovalLayer");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CardApprovalLayer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CardApprovalLayer");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CardApprovalLayer");
        }
    }
}
