using TradingApp.Core.Models.Stocks;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

public class StockService : IStockService
{
    private readonly IStockRepository stockRepository;

    public StockService(IStockRepository stockRepository)
    {
        this.stockRepository = stockRepository;
    }

    public async Task<IEnumerable<Stock>> GetAll()
    {
        var stocks = await stockRepository.GetAllAsync();

        if (stocks is null)
        {
            return Enumerable.Empty<Stock>();
        }

        return stocks;
    }

    public async Task<IEnumerable<StockOHLC>> GetStockOHLC(string id)
    {

        var stocks = await stockRepository.GetStockOHLC(id);

        if (stocks is null)
        {
            return Enumerable.Empty<StockOHLC>();
        }

        return stocks;
    }

    public async Task<IEnumerable<StockPriceHistory>> GetStockPriceHistory(string id, string timestamp = "24h")
    {

        var stocks = await stockRepository.GetStockPriceHistory(id, timestamp);

        if (stocks is null)
        {
            return Enumerable.Empty<StockPriceHistory>();
        }

        return stocks;
    }

    public async Task<IEnumerable<Stock>> GetAllWithOffsetAsync(int offset)
    {
        if (offset < 0)
        {
            throw new ArgumentException("Offset cannot be negative");
        }

        var stocks = await stockRepository.GetAllWithOffsetAsync(offset);

        if (stocks is null)
        {
            return Enumerable.Empty<Stock>();
        }

        return stocks;
    }

    public async Task<Stock> GetByIdAsync(string id)
    {

        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Id cannot be empty");
        }

        var stock = await stockRepository.GetByIdAsync(id);

        if (stock is null)
        {
            throw new NullReferenceException("Stock not found");
        }

        return stock;
    }


}
