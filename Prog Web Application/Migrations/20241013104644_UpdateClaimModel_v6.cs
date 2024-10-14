using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog_Web_Application.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaimModel_v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "submissionDate",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "submissionDate",
                table: "Claims");
        }
    }
}
