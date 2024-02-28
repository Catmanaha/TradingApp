using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository auctionRepository;
    private readonly IStockRepository stockRepository;
    private readonly UserManager<User> userManager;

    public AuctionService(
        IAuctionRepository auctionRepository,
        IStockRepository stockRepository,
        UserManager<User> userManager
    )
    {
        this.auctionRepository = auctionRepository;
        this.stockRepository = stockRepository;
        this.userManager = userManager;
    }

    public async Task<IEnumerable<AuctionForUser>> GetAllForUser(int id)
    {
        var auctions = await auctionRepository.GetAllForUserAsync(id);
        var user = await userManager.Users.FirstOrDefaultAsync(o => o.Id == id);
        var auctionForUsers = new List<AuctionForUser>();

        foreach (var auction in auctions)
        {
            var stock = await stockRepository.GetByIdAsync(auction.StockUuid);

            auctionForUsers.Add(new AuctionForUser
                    {
                        StockName = stock.Name,
                        AuctionInitialPrice = auction.InitialPrice,
                        UserName = user.UserName,
                        AuctionStatus = auction.Status,
                        AuctionStartTime = auction.StartTime,
                        AuctionEndTime = auction.EndTime,
                        AuctionId = auction.Id
                    });
        }

        return auctionForUsers;
    }

    public async Task<IEnumerable<AuctionForUser>> GetAllForView()
    {
        var query = from auction in await auctionRepository.GetAllAsync()
                    join user in await userManager.Users.ToListAsync() on auction.UserId equals user.Id
                    join stock in await stockRepository.GetAllAsync() on auction.StockUuid equals stock.Uuid
                    select new AuctionForUser
                    {
                        StockName = stock.Name,
                        AuctionInitialPrice = auction.InitialPrice,
                        UserName = user.UserName,
                        AuctionStatus = auction.Status,
                        AuctionStartTime = auction.StartTime,
                        AuctionEndTime = auction.EndTime,
                        AuctionId = auction.Id
                    };

        return query;
    }
}
