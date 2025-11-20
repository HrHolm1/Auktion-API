using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auktion_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBackToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LotNumber",
                schema: "auctions",
                table: "Lots",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LotNumber",
                schema: "auctions",
                table: "Lots",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
