using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassLibrary.Migrations
{
    public partial class initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "Role",
                column: "Name",
                value: "Admin");

            migrationBuilder.InsertData(
                table: "Role",
                column: "Name",
                value: "User");

            migrationBuilder.InsertData(
                table: "Role",
                column: "Name",
                value: "Guest");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleName",
                table: "User",
                column: "RoleName");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleName",
                table: "User",
                column: "RoleName",
                principalTable: "Role",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleName",
                table: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "User");
        }
    }
}
