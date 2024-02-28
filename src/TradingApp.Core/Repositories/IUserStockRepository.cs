using TradingApp.Core.Models;
using TradingApp.Core.Repositories.Base;

namespace TradingApp.Core.Repositories;

public interface IUserStockRepository : ICreate<UserStock>,
                                        IGetById<UserStock, int>,
                                        IUpdate<UserStock>,
                                        IGetAll<UserStock>,
                                        IGetAllById<UserStock>
{
    public Task Sell(UserStock userStock, double count);
    public Task<IEnumerable<UserStock>> GetAllForUserAsync(int id);
};