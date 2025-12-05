using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auktion_API.Migrations
{
    /// <inheritdoc />
    public partial class AddLotImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LotImages",
                schema: "auctions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LotId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotImages_Lots_LotId",
                        column: x => x.LotId,
                        principalSchema: "auctions",
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LotImages_LotId",
                schema: "auctions",
                table: "LotImages",
                column: "LotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LotImages",
                schema: "auctions");
        }
    }
}
