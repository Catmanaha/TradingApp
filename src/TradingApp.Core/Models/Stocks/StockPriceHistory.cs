using Newtonsoft.Json;

namespace TradingApp.Core.Models.Stocks;
public class StockPriceHistory
{
    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("timestamp")]
    public long TimeStamp { get; set; }

    public string? DateTime
    {
        get => DateTimeOffset.FromUnixTimeSeconds(TimeStamp).DateTime.ToString("MMM dd, hh:mm tt, yyyy");
    }
}
