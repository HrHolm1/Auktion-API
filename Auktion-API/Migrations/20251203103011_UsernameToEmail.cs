using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auktion_API.Migrations
{
    /// <inheritdoc />
    public partial class UsernameToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                schema: "auctions",
                table: "Users",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "auctions",
                table: "Users",
                newName: "Username");
        }
    }
}
