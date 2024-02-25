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

    public async Task<Stock> CreateAsync(Stock stock)
    {
        await DBC.Stocks.AddAsync(stock);
        await DBC.SaveChangesAsync();

        return stock;
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await DBC.Stocks.ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await DBC.Stocks.FirstOrDefaultAsync(o => o.Id == id);
    }

    public IEnumerable<Stock> GetRecentStocks()
    {
        return DBC.Stocks.AsEnumerable().TakeLast(5);
    }

    public async Task UpdateAsync(Stock model)
    {
        DBC.Stocks.Update(model);
        await DBC.SaveChangesAsync();
    }
}