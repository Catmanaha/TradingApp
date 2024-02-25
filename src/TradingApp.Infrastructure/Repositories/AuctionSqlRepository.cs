using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;
using TradingApp.Infrastructure.Extensions;

namespace TradingApp.Infrastructure.Repositories;

public class AuctionSqlRepository : IAuctionRepository
{
    private readonly TradingAppDbContext DBC;

    public AuctionSqlRepository(TradingAppDbContext DBC)
    {
        this.DBC = DBC;
    }

    public async Task<Auction> CreateAsync(Auction model)
    {
        await DBC.Auctions.AddAsync(model);
        await DBC.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<object>> GetAllAsync()
    {
        var query = from auction in await DBC.Auctions.ToListAsync()
                    join user in await DBC.Users.ToListAsync() on auction.UserId equals user.Id
                    join stock in await DBC.Stocks.ToListAsync() on auction.StockId equals stock.Id
                    select new
                    {
                        StockName = stock.Name,
                        auction.InitialPrice,
                        user.UserName,
                        auction.Status,
                        auction.StartTime,
                        auction.EndTime,
                        auction.Id
                    };

        return query.ToExpandoObjectCollection();
    }

    public async Task<IEnumerable<object>> GetAllForUser(int id)
    {
        var query = from auction in await DBC.Auctions.ToListAsync()
                    join stock in await DBC.Stocks.ToListAsync() on auction.StockId equals stock.Id
                    join user in await DBC.Users.Where(o => o.Id == id).ToListAsync() on auction.UserId equals user.Id
                    select new
                    {
                        StockName = stock.Name,
                        auction.InitialPrice,
                        user.UserName,
                        auction.Status,
                        auction.StartTime,
                        auction.EndTime,
                        auction.Id
                    };

        return query.ToExpandoObjectCollection();
    }

    public async Task<Auction?> GetByIdAsync(int id)
    {
        return await DBC.Auctions.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateAsync(Auction model)
    {
        DBC.Auctions.Update(model);
        await DBC.SaveChangesAsync();
    }
}