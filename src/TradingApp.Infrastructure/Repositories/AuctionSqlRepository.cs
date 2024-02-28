using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;

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

    public async Task<IEnumerable<Auction>> GetAllAsync() {
        return await DBC.Auctions.ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetAllByIdAsync(int id)
    {
        return await DBC.Auctions.Where(o => o.Id == id).ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetAllForUserAsync(int id)
    {
        return await DBC.Auctions.Where(o => o.UserId == id).ToListAsync();
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