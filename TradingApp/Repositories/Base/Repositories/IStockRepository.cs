using TradingApp.Models;

namespace TradingApp.Repositories.Base;

public interface IStockRepository : IGetAll<Stock>, ICreate<Stock>
{
    public Task<IEnumerable<Stock>> GetRecentStocks();
}
