using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prof_tester_api.Migrations
{
    /// <inheritdoc />
    public partial class lecterns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LecternModel_Organizations_OrganizationId",
                table: "LecternModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LecternModel",
                table: "LecternModel");

            migrationBuilder.RenameTable(
                name: "LecternModel",
                newName: "Lecterns");

            migrationBuilder.RenameIndex(
                name: "IX_LecternModel_OrganizationId",
                table: "Lecterns",
                newName: "IX_Lecterns_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecterns",
                table: "Lecterns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecterns_Organizations_OrganizationId",
                table: "Lecterns",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecterns_Organizations_OrganizationId",
                table: "Lecterns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecterns",
                table: "Lecterns");

            migrationBuilder.RenameTable(
                name: "Lecterns",
                newName: "LecternModel");

            migrationBuilder.RenameIndex(
                name: "IX_Lecterns_OrganizationId",
                table: "LecternModel",
                newName: "IX_LecternModel_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LecternModel",
                table: "LecternModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LecternModel_Organizations_OrganizationId",
                table: "LecternModel",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
