namespace TradingApp.Models;

public class UserStock
{
    public int UserId { get; set; }
    public int StockId { get; set; }
    public int StockCount { get; set; }

    public UserStock(int userId, int stockId, int stockCount)
    {
        this.UserId = userId;
        this.StockId = stockId;
        this.StockCount = stockCount;
    }
}