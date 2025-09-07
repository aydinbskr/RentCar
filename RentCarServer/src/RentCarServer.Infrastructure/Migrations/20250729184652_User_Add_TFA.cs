using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class User_Add_TFA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TFACode",
                table: "Users",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TFAConfirmCode",
                table: "Users",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TFAExpiresDate",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TFAIsCompleted",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TFAStatus",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TFACode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAConfirmCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAExpiresDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAIsCompleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TFAStatus",
                table: "Users");
        }
    }
}
