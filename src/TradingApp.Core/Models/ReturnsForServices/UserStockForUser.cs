namespace TradingApp.Core.Models.ReturnsForServices;

public class UserStockForUser
{
    public int UserStockId { get; set; }
    public string? StockUuid { get; set; }
    public string? StockIconUrl { get; set; }
    public string? StockName { get; set; }
    public double UserStockTotalPrice { get; set; }
    public double StockCount { get; set; }
}