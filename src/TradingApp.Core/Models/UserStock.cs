namespace TradingApp.Core.Models;

public class UserStock
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StockId { get; set; }
    public string? StockName { get; set; }
    public int StockCount { get; set; }
}