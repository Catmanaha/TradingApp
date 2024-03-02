namespace TradingApp.Core.Models;

public class UserStock
{
    public int Id { get; set; }
    public string? StockUuid { get; set; }
    public int UserId { get; set; }
    public double TotalPrice { get; set; }
    public double StockCount { get; set; }
}
