using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;
using TradingApp.Infrastructure.Extensions;

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
        var stock = await DBC.UserStocks.FirstOrDefaultAsync(o => o.UserId == model.UserId && o.StockId == model.StockId);
        if (stock is null)
        {
            await DBC.UserStocks.AddAsync(model);
        }
        else
        {
            stock.StockCount += model.StockCount;
            stock.TotalPrice += model.TotalPrice;

            DBC.UserStocks.Update(stock);
        }

        await DBC.SaveChangesAsync();
        return model;
    }

    public async Task<IEnumerable<object>> GetAllForUser(int id)
    {
        var query = from stock in await DBC.Stocks.ToListAsync()
                    join userStock in await DBC.UserStocks.ToListAsync() on stock.Id equals userStock.StockId
                    join user in await DBC.Users.Where(o => o.Id == id).ToListAsync() on userStock.UserId equals user.Id
                    select new { StockId = stock.Id, userStock.Id, stock.ImageUrl, stock.Name, userStock.TotalPrice, userStock.StockCount };
        
        return query.Distinct().ToExpandoObjectCollection();
    }

    public async Task<UserStock?> GetByIdAsync(int id)
    {
        return await DBC.UserStocks.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task Sell(UserStock userStock, int count)
    {
        var result = await DBC.UserStocks.FirstOrDefaultAsync(o => o.Id == userStock.Id);

        var totalCount = result.StockCount - count;

        if (totalCount == 0)
        {
            DBC.UserStocks.Remove(userStock);
            await DBC.SaveChangesAsync();
            return;
        }

        result.StockCount -= count;

        DBC.UserStocks.Update(result);
        await DBC.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserStock model)
    {
        DBC.UserStocks.Update(model);
        await DBC.SaveChangesAsync();
    }
}
