namespace TradingApp.Models;

public class Stock
{
    public Stock(int id, string? symbol, string? name, string? marketCap)
    {
        Id = id;
        Symbol = symbol;
        Name = name;
        MarketCap = marketCap;
    }

    public int Id { get; set; }
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public string? MarketCap { get; set; }
}