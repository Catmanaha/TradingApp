using TradingApp.Models;

namespace TradingApp.Repositories.Base;

public interface IStockRepository : IGetAll<Stock>, ICreate<Stock> { }
