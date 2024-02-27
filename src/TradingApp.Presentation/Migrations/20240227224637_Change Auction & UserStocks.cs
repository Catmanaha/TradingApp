using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuctionUserStocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockSymbol",
                table: "UserStocks",
                newName: "StockUuid");

            migrationBuilder.RenameColumn(
                name: "StockSymbol",
                table: "Auctions",
                newName: "StockUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockUuid",
                table: "UserStocks",
                newName: "StockSymbol");

            migrationBuilder.RenameColumn(
                name: "StockUuid",
                table: "Auctions",
                newName: "StockSymbol");
        }
    }
}
