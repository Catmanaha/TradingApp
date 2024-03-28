namespace TradingApp.Core.Models.Configurations;

public class StockApiConfiguration
{
    public string? BaseUrl { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
}
