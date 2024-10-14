using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog_Web_Application.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaimModel_v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Claims",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Claims",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Claims",
                type: "TEXT",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Claims",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "Claims",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Claims",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
