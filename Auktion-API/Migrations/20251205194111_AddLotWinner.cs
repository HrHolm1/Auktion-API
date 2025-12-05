using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auktion_API.Migrations
{
    /// <inheritdoc />
    public partial class AddLotWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerUserId",
                schema: "auctions",
                table: "Lots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_WinnerUserId",
                schema: "auctions",
                table: "Lots",
                column: "WinnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Users_WinnerUserId",
                schema: "auctions",
                table: "Lots",
                column: "WinnerUserId",
                principalSchema: "auctions",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Users_WinnerUserId",
                schema: "auctions",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_WinnerUserId",
                schema: "auctions",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "WinnerUserId",
                schema: "auctions",
                table: "Lots");
        }
    }
}
