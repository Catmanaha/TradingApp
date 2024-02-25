namespace TradingApp.Core.Models;

public class Stock
{
    public int Id { get; set; }
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public long MarketCap { get; set; }
    public string? ImageUrl { get; set; }
}