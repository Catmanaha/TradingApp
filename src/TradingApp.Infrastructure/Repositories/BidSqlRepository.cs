using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;

namespace TradingApp.Infrastructure.Repositories;

public class BidSqlRepository : IBidRepository
{
    private readonly TradingAppDbContext DBC;

    public BidSqlRepository(TradingAppDbContext DBC)
    {
        this.DBC = DBC;
    }

    public async Task<Bid> CreateAsync(Bid model)
    {
        await DBC.Bids.AddAsync(model);
        await DBC.SaveChangesAsync();

        return model;
    }

    public async Task<IEnumerable<Bid>> GetAllAsync()
    {
        return await DBC.Bids.ToListAsync();
    }

    public async Task<Bid?> GetHighestBidForAuction(int auctionId)
    {
        if (DBC.Bids.Where(o => o.AuctionId == auctionId).Any() == false)
        {
            return null;
        }

        var maxBidAmount = await DBC.Bids.Where(o => o.AuctionId == auctionId).MaxAsync(o => o.BidAmount);
        return await DBC.Bids.FirstOrDefaultAsync(o => o.AuctionId == auctionId && o.BidAmount == maxBidAmount);
    }
}
