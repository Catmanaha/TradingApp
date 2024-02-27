using Newtonsoft.Json;

namespace TradingApp.Core.Models
{
    public class StockPriceHistory
    {
        [JsonProperty("price")]
        public string Price { get; set; }
        
        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }

        [JsonIgnore]
        public DateTime DateTime{
            get => DateTimeOffset.FromUnixTimeSeconds(TimeStamp).DateTime;
        }
    }
}