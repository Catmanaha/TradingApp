using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalPricetoUserStocksRemoveStockName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockName",
                table: "UserStocks");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "UserStocks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "UserStocks");

            migrationBuilder.AddColumn<string>(
                name: "StockName",
                table: "UserStocks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
