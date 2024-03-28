using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TradingApp.Core.Models.Configurations;
using TradingApp.Core.Models.Stocks;
using TradingApp.Core.Repositories;

namespace TradingApp.Infrastructure.Repositories;

public class StockApiRepository : IStockRepository
{
    private readonly HttpClient client;

    public StockApiRepository(IOptions<StockApiConfiguration> options, HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri(options.Value.BaseUrl);

        foreach (var header in options.Value.Headers)
        {
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

    }

    public async Task<IEnumerable<Stock>?> GetAllWithOffsetAsync(int offset)
    {
        var limit = 20;
        return await Deserialize<IEnumerable<Stock>>(StockApiRequests.GetAll(limit, limit * offset), "coins");
    }

    public async Task<Stock?> GetByIdAsync(string id)
    {
        return await Deserialize<Stock?>(StockApiRequests.GetById(id), "coin");
    }

    public async Task<IEnumerable<StockPriceHistory>?> GetStockPriceHistory(string id, string timestamp = "24h")
    {
        return await Deserialize<IEnumerable<StockPriceHistory>>(StockApiRequests.StockPriceHistory(id, timestamp), "history");
    }

    public async Task<IEnumerable<StockOHLC>?> GetStockOHLC(string id)
    {
        return await Deserialize<IEnumerable<StockOHLC>>(StockApiRequests.GetStockOHLC(id), "ohlc");
    }

    public async Task<IEnumerable<Stock>?> GetAllAsync()
    {
        return await Deserialize<IEnumerable<Stock>>(StockApiRequests.GetAll(), "coins");
    }

    private async Task<T?> Deserialize<T>(string url, string value)
    {
        var result = await client.GetAsync(url);
        var json = await result.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var element = parsed["data"][value];

        return element.ToObject<T>();
    }
}