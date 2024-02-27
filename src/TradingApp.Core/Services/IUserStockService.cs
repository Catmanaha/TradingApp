using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Services;

public interface IUserStockService : ICreate<UserStock>
{
    public Task<IEnumerable<UserStockForUser>> GetAllForUser(int id);
}
