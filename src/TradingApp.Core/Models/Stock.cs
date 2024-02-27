using Newtonsoft.Json;

namespace TradingApp.Core.Models;

public class Stock
{
    [JsonProperty("uuid")]
    public string Uuid { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }

    [JsonProperty("iconUrl")]
    public string IconUrl { get; set; }

    [JsonProperty("marketCap")]
    public long MarketCap { get; set; }

    [JsonProperty("price")]
    public double Price { get; set; }

    [JsonProperty("change")]
    public double Change { get; set; }

    [JsonProperty("24hVolume")]
    public long _24hVolume { get; set; }

}