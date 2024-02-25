using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IUserStockRepository : ICreate<UserStock>
{
    public IEnumerable<object> GetAllForUser(int id);
};