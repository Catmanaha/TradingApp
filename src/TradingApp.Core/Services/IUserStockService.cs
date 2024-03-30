using TradingApp.Core.Dtos;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Services;

public interface IUserStockService
{
    public Task<IEnumerable<UserStockForUser>> GetAllForUser(int id);
    public Task<string> Sell(SellUserStockDto dto);
    public Task<string> CreateAsync(UserStockDto userStockDto, User user);
    public Task<UserStock> GetById(int id);
}
