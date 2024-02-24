using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IStockRepository : IGetAll<Stock>, ICreate<Stock>
{
    public IEnumerable<Stock> GetRecentStocks();
}
