using Newtonsoft.Json;

namespace TradingApp.Core.Models.Stocks;

public class StockOHLC
{
    [JsonProperty("startingAt")]
    public long StartingAtTimeSpan { get; set; }

    [JsonProperty("endingAt")]
    public long EndingAtTimeSpan { get; set; }

    public string? EndingAtDateTime
    {
        get => DateTimeOffset.FromUnixTimeSeconds(EndingAtTimeSpan).DateTime.ToString("MMM dd, hh:mm tt, yyyy");
    }

    public string? StartingAtDateTime
    {
        get => DateTimeOffset.FromUnixTimeSeconds(StartingAtTimeSpan).DateTime.ToString("MMM dd, hh:mm tt, yyyy");
    }

    [JsonProperty("open")]
    public string Open { get; set; }

    [JsonProperty("close")]
    public double Close { get; set; }

    [JsonProperty("high")]
    public double High { get; set; }

    [JsonProperty("low")]
    public double Low { get; set; }

    [JsonProperty("avg")]
    public double Average { get; set; }
}