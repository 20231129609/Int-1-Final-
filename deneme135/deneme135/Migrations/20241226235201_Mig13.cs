using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace deneme135.Migrations
{
    /// <inheritdoc />
    public partial class Mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_ApplicationUser_ApplicationUserId",
                table: "TestResults");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "TestResults",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestResults_ApplicationUserId",
                table: "TestResults",
                newName: "IX_TestResults_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_AspNetUsers_UserId",
                table: "TestResults",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_AspNetUsers_UserId",
                table: "TestResults");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TestResults",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults",
                newName: "IX_TestResults_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_ApplicationUser_ApplicationUserId",
                table: "TestResults",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
