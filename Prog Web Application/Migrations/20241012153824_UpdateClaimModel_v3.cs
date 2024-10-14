using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog_Web_Application.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaimModel_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Claims",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "HoursWorked",
                table: "Claims",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Claims",
                newName: "UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "HoursWorked",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
