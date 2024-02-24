using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;

namespace TradingApp.Infrastructure.Repositories;

public class StockSqlRepository : IStockRepository
{
    private readonly TradingAppDbContext DBC;

    public StockSqlRepository(TradingAppDbContext DBC)
    {
        this.DBC = DBC;
    }

    public async Task CreateAsync(Stock stock)
    {
        await DBC.Stocks.AddAsync(stock);
        await DBC.SaveChangesAsync();
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await DBC.Stocks.ToListAsync();
    }

    public IEnumerable<Stock> GetRecentStocks()
    {
        return DBC.Stocks.AsEnumerable().TakeLast(5);
    }

}