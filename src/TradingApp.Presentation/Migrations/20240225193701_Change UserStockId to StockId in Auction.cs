using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserStockIdtoStockIdinAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserStockId",
                table: "Auctions",
                newName: "StockId");

            migrationBuilder.AlterColumn<double>(
                name: "StockCount",
                table: "UserStocks",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "Auctions",
                newName: "UserStockId");

            migrationBuilder.AlterColumn<int>(
                name: "StockCount",
                table: "UserStocks",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
