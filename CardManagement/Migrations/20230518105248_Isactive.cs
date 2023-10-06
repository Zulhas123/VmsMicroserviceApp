using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardManagement.Migrations
{
    /// <inheritdoc />
    public partial class Isactive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CardApplication",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "CardApplication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CardApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "CardApplication",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CardApplication",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CardApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "CardApplication",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CardApplication");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CardApplication");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CardApplication");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CardApplication");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CardApplication");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CardApplication");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CardApplication");
        }
    }
}
