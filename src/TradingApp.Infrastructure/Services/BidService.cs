using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

public class BidService : IBidService
{
    private readonly IBidRepository bidRepository;
    private readonly IAuctionRepository auctionRepository;
    private readonly UserManager<User> userManager;

    public BidService(
        IBidRepository bidRepository,
        IAuctionRepository auctionRepository,
        UserManager<User> userManager
    )
    {
        this.bidRepository = bidRepository;
        this.auctionRepository = auctionRepository;
        this.userManager = userManager;
    }


    public async Task<IEnumerable<BidForAuction>> GetAllForAuction(int id)
    {
        var query = from bid in await bidRepository.GetAllAsync()
                    join auction in await auctionRepository.GetAllByIdAsync(id) on bid.AuctionId equals auction.Id
                    join user in await userManager.Users.ToListAsync() on bid.UserId equals user.Id
                    select new BidForAuction
                    {
                        BidTime = bid.BidTime,
                        BidAmount = bid.BidAmount,
                        UserName = user.UserName,
                        AuctionId = bid.AuctionId
                    };

        return query;
    }

}
