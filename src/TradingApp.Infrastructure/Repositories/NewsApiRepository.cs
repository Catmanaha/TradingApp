using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TradingApp.Core.Models;
using TradingApp.Core.Models.Configurations;
using TradingApp.Core.Repositories;

namespace TradingApp.Infrastructure.Repositories;

public class NewsApiRepository : INewsRepository
{
    private readonly HttpClient client;
    private readonly string apiKey;

    public NewsApiRepository(IOptions<NewsApiConfiguration> options, HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri(options.Value.BaseUrl);
        apiKey = options.Value.ApiKey;
    }

    public async Task<News> GetNews(int offset = 0)
    {
        int limit = 3;
        var result = await client.GetAsync($"news/all?language=en&api_token={apiKey}&page={limit * offset}");
        var json = await result.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);

        var articles = parsed["data"].ToObject<IEnumerable<Article>>();

        return new News{
            Articles = articles,
            Offset = offset
        };
    }
}
