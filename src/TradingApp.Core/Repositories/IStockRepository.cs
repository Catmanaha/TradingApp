using TradingApp.Core.Models.Stocks;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IStockRepository : IGetById<Stock, string>, IGetAll<Stock>
{
    public Task<IEnumerable<Stock>> GetAllWithOffsetAsync(int offset);
    public Task<IEnumerable<StockPriceHistory>> GetStockPriceHistory(string id, string timestamp = "24h");
    public Task<IEnumerable<StockOHLC>> GetStockOHLC(string id);
}
