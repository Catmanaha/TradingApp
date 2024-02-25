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
            await DBC.SaveChangesAsync();
        }
        else
        {
            stock.StockCount += model.StockCount;
            stock.TotalPrice += model.TotalPrice;

            DBC.UserStocks.Update(stock);
            await DBC.SaveChangesAsync();
        }

        return model;
    }

    public IEnumerable<object> GetAllForUser(int id)
    {
        var query = from stock in DBC.Stocks.ToList()
                    join userStock in DBC.UserStocks.ToList() on stock.Id equals userStock.StockId
                    join user in DBC.Users.ToList() on userStock.UserId equals user.Id
                    select new { stock.ImageUrl, stock.Name, userStock.TotalPrice, userStock.StockCount };

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
}
