using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auktion_API.Migrations
{
    /// <inheritdoc />
    public partial class DefaultStartPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "StartingPrice",
                schema: "auctions",
                table: "Lots",
                type: "float",
                nullable: false,
                defaultValue: 1.0,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "StartingPrice",
                schema: "auctions",
                table: "Lots",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 1.0);
        }
    }
}
