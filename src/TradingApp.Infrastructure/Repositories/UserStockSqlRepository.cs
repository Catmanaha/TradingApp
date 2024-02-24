using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;

namespace TradingApp.Infrastructure.Repositories;

public class UserStockSqlRepository : IUserStockRepository
{
    private readonly TradingAppDbContext DBC;

    public UserStockSqlRepository(TradingAppDbContext DBC)
    {
        this.DBC = DBC;
    }

    public async Task CreateAsync(UserStock model)
    {
        await DBC.UserStocks.AddAsync(model);
        await DBC.SaveChangesAsync();
    }

    public IEnumerable<UserStock> GetAllForUser(int id)
    {
        return DBC.UserStocks.Where(u => u.UserId == id);
    }
}
