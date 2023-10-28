using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prof_tester_api.Migrations
{
    /// <inheritdoc />
    public partial class iir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WasPasswordResetRequest",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Users",
                newName: "FilenameIcon");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "IX_Users_Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Token",
                table: "Users",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Token",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "FilenameIcon",
                table: "Users",
                newName: "Image");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Phone",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddColumn<bool>(
                name: "WasPasswordResetRequest",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Token",
                table: "Users",
                column: "Token",
                unique: true);
        }
    }
}
