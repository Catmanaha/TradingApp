using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Data;
using TradingApp.Infrastructure.Extensions;

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

    public async Task<IEnumerable<object>> GetAllForAuction(int id)
    {
        var query = from bid in await DBC.Bids.ToListAsync()
                    join auction in await DBC.Auctions.Where(o => o.Id == id).ToListAsync() on bid.AuctionId equals auction.Id
                    join user in await DBC.Users.ToListAsync() on bid.UserId equals user.Id
                    select new
                    {
                        bid.BidTime,
                        bid.BidAmount,
                        user.UserName,
                        bid.AuctionId
                    };

        return query.ToExpandoObjectCollection();
    }

    public async Task<Bid?> GetHighestBidForAuction(int auctionId)
    {
        if (DBC.Bids.Any() == false)
        {
            return null;
        }

        var maxBidAmount = await DBC.Bids.MaxAsync(o => o.BidAmount);
        return await DBC.Bids.FirstOrDefaultAsync(o => o.AuctionId == auctionId && o.BidAmount == maxBidAmount);
    }
}
