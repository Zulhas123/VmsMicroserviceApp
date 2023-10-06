using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Migrations
{
    /// <inheritdoc />
    public partial class UserTableModifiedAndAPICreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "UserInfos");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "UserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "UserInfos");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "UserInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
