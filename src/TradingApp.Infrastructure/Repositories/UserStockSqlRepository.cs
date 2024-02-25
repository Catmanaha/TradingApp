using System.ComponentModel;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        var stock = await DBC.UserStocks.Where(o => o.UserId == model.UserId && o.StockId == model.StockId).FirstOrDefaultAsync();
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

    public IEnumerable<object> GetAllForUser(int id)
    {
        var query = from stock in DBC.Stocks.ToList()
                    join userStock in DBC.UserStocks.ToList() on stock.Id equals userStock.StockId
                    join user in DBC.Users.ToList() on userStock.UserId equals id
                    select new { userStock.Id, stock.ImageUrl, stock.Name, userStock.TotalPrice, userStock.StockCount };

        var joinData = new List<ExpandoObject>();
        foreach (var item in query)
        {
            IDictionary<string, object> itemExpando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item.GetType()))
            {
                itemExpando.Add(property.Name, property.GetValue(item));
            }
            joinData.Add(itemExpando as ExpandoObject);
        }

        return joinData;


    }

    public async Task<UserStock?> GetByIdAsync(int id)
    {
        return await DBC.UserStocks.Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task Sell(UserStock userStock, int count)
    {
        var result = await DBC.UserStocks.Where(o => o.Id == userStock.Id).FirstOrDefaultAsync();

        var totalCount =  result.StockCount - count;

        if (totalCount == 0) {
            DBC.UserStocks.Remove(userStock);
            await DBC.SaveChangesAsync();
            return;
        }

        result.StockCount -= count;

        DBC.UserStocks.Update(result);
        await DBC.SaveChangesAsync();
    }
}
