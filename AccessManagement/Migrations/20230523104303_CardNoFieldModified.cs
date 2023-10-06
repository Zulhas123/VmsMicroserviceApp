using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessManagement.Migrations
{
    /// <inheritdoc />
    public partial class CardNoFieldModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CardNo",
                table: "UserAccess",
                newName: "UHFCardNo");

            migrationBuilder.AddColumn<string>(
                name: "HFCardNo",
                table: "UserAccess",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HFCardNo",
                table: "UserAccess");

            migrationBuilder.RenameColumn(
                name: "UHFCardNo",
                table: "UserAccess",
                newName: "CardNo");
        }
    }
}
