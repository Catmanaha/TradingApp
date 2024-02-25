namespace TradingApp.Core.Models;

public class UserStock
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StockId { get; set; }
    public double TotalPrice { get; set; }
    public double StockCount { get; set; }
}
