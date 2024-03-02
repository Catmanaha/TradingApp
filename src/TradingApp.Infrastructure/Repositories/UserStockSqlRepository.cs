using Microsoft.EntityFrameworkCore;
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

    public async Task<UserStock> CreateAsync(UserStock model)
    {
        await DBC.UserStocks.AddAsync(model);
        await DBC.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<UserStock>> GetAllAsync()
    {
        return await DBC.UserStocks.ToListAsync();
    }

    public async Task<IEnumerable<UserStock>> GetAllByIdAsync(int id)
    {
        return await DBC.UserStocks.Where(o => o.Id == id).ToListAsync();
    }

    public async Task<IEnumerable<UserStock>> GetAllForUserAsync(int id)
    {
        return await DBC.UserStocks.Where(o => o.UserId == id).ToListAsync();
    }

    public async Task<UserStock?> GetByIdAsync(int id)
    {
        return await DBC.UserStocks.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task Sell(UserStock userStock, double count)
    {
        var totalCount = userStock.StockCount - count;

        if (totalCount == 0)
        {
            DBC.UserStocks.Remove(userStock);
            await DBC.SaveChangesAsync();
            return;
        }

        userStock.TotalPrice = (userStock.TotalPrice / userStock.StockCount) * (userStock.StockCount - count);
        userStock.StockCount -= count;

        DBC.UserStocks.Update(userStock);
        await DBC.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserStock model)
    {
        DBC.UserStocks.Update(model);
        await DBC.SaveChangesAsync();
    }
}
