using Newtonsoft.Json;

namespace TradingApp.Core.Models;

public class Article
{
    [JsonProperty("title")]
    public string? Title { get; set; }
    [JsonProperty("url")]
    public string? Link { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }
    [JsonProperty("published_at")]
    public DateTime? PublishDate { get; set; }
    [JsonProperty("image_url")]
    public string? ImgUrl { get; set; }
}
