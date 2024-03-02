using TradingApp.Core.Models.Stocks;

namespace TradingApp.Core.Services;

public interface IStockService
{
    public Task<Stock> GetByIdAsync(string id);
    public Task<IEnumerable<Stock>> GetAll();
    public Task<IEnumerable<Stock>> GetAllWithOffsetAsync(int offset);
    public Task<IEnumerable<StockPriceHistory>> GetStockPriceHistory(string id, string timestamp = "24h");
    public Task<IEnumerable<StockOHLC>> GetStockOHLC(string id);
}
