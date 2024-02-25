using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialPricetoAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "Auctions",
                newName: "UserStockId");

            migrationBuilder.AddColumn<double>(
                name: "InitialPrice",
                table: "Auctions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialPrice",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "UserStockId",
                table: "Auctions",
                newName: "StockId");
        }
    }
}
