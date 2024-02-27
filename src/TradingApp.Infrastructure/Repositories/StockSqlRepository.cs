using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;

namespace TradingApp.Infrastructure.Repositories;

public class StockSqlRepository : IStockRepository
{
    private string apiUrl = "https://coinranking1.p.rapidapi.com/";
    private readonly HttpClient client;

    public StockSqlRepository(HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "coinranking1.p.rapidapi.com");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "323a9b5888mshce7c554fb2fab1fp11f674jsnf6a933f94130");
    }

    public async Task<IEnumerable<Stock>> GetAllForViewAsync(int offset)
    {
        var limit = 20;
        var stocks = await client.GetAsync(StockApiRequests.GetAll(limit, limit * offset));
        var json = await stocks.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var results = parsed["data"]["coins"];
        var stocksDeserialized = results.ToObject<IEnumerable<Stock>>();

        return stocksDeserialized;
    }

    public async Task<Stock?> GetByIdAsync(string id)
    {
        var stock = await client.GetAsync(StockApiRequests.GetById(id));
        var json = await stock.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var result = parsed["data"]["coin"];

        return result.ToObject<Stock>();
    }

    public async Task<IEnumerable<StockPriceHistory>> GetStockPriceHistory(string id, string timestamp = "24h")
    {
        var stock = await client.GetAsync(StockApiRequests.StockPriceHistory(id, timestamp));
        var json = await stock.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var result = parsed["data"]["history"];

        return result.ToObject<IEnumerable<StockPriceHistory>>();
    }

    public async Task<IEnumerable<StockOHLC>> GetStockOHLC(string id)
    {
        var stock = await client.GetAsync(StockApiRequests.GetStockOHLC(id));
        var json = await stock.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var result = parsed["data"]["ohlc"];

        return result.ToObject<IEnumerable<StockOHLC>>();
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        var stocks = await client.GetAsync(StockApiRequests.GetAll());
        var json = await stocks.Content.ReadAsStringAsync();
        var parsed = JObject.Parse(json);
        var results = parsed["data"]["coins"];
        var stocksDeserialized = results.ToObject<IEnumerable<Stock>>();

        return stocksDeserialized;
    }
}