using Newtonsoft.Json;

namespace TradingApp.Core.Models;

public class StockOHLC
{
    [JsonProperty("startingAt")]
    public long StartingAtTimeSpan { get; set; }

    [JsonProperty("endingAt")]
    public long EndingAtTimeSpan { get; set; }

    [JsonIgnore]
    public DateTime EndingAtDateTime
    {
        get => DateTimeOffset.FromUnixTimeSeconds(EndingAtTimeSpan).DateTime;
    }

    [JsonIgnore]
    public DateTime StartingAtDateTime
    {
        get => DateTimeOffset.FromUnixTimeSeconds(StartingAtTimeSpan).DateTime;
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
