using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGames.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserSubscribeBirthday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewEmail",
                table: "UserTokens",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribe",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewEmail",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSubscribe",
                table: "Users");
        }
    }
}
