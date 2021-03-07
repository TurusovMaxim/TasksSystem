using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassLibrary.Migrations
{
    public partial class initial8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.AddColumn<byte[]>(
                name: "AesIV",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AesKey",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "encryptedBirthday",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "encryptedComment",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "encryptedFirstName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "encryptedLastName",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AesIV",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AesKey",
                table: "User");

            migrationBuilder.DropColumn(
                name: "encryptedBirthday",
                table: "User");

            migrationBuilder.DropColumn(
                name: "encryptedComment",
                table: "User");

            migrationBuilder.DropColumn(
                name: "encryptedFirstName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "encryptedLastName",
                table: "User");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
