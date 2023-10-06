using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorManagement.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToWhome",
                table: "VisitorApplication",
                newName: "ToWhom");

            migrationBuilder.AlterColumn<int>(
                name: "VisitorConfiugurtationId",
                table: "VisitorData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "VisitorData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "VisitorApplication",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "VisitorApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "VisitorApplication",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFrontDesk",
                table: "VisitorApplication",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RejectedBy",
                table: "VisitorApplication",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VisitorApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "VisitorApplication",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "Visitor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Visitor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "VisitorApplication");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "VisitorApplication");

            migrationBuilder.DropColumn(
                name: "IsFrontDesk",
                table: "VisitorApplication");

            migrationBuilder.DropColumn(
                name: "RejectedBy",
                table: "VisitorApplication");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VisitorApplication");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "VisitorApplication");

            migrationBuilder.RenameColumn(
                name: "ToWhom",
                table: "VisitorApplication",
                newName: "ToWhome");

            migrationBuilder.AlterColumn<int>(
                name: "VisitorConfiugurtationId",
                table: "VisitorData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "VisitorData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "VisitorApplication",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "Visitor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Visitor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
