using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Enums;
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
    private readonly IBidRepository bidRepository;
    private readonly IUserStockService userStockService;
    private readonly IUserStockRepository userStockRepository;

    public AuctionService(
        IAuctionRepository auctionRepository,
        IStockRepository stockRepository,
        UserManager<User> userManager,
        IBidRepository bidRepository,
        IUserStockService userStockService,
        IUserStockRepository userStockRepository
    )
    {
        this.auctionRepository = auctionRepository;
        this.stockRepository = stockRepository;
        this.userManager = userManager;
        this.bidRepository = bidRepository;
        this.userStockService = userStockService;
        this.userStockRepository = userStockRepository;
    }

    public async Task ChangeStatus(AuctionStatusEnum status, int id)
    {
        var auction = await auctionRepository.GetByIdAsync(id);
        auction.Status = status;


        if (status == AuctionStatusEnum.Closed)
        {
            var highestBid = await bidRepository.GetHighestBidForAuction(id);

            if (highestBid is null)
            {
                await auctionRepository.UpdateAsync(auction);

                var userAuctionn = await userManager.FindByIdAsync(auction.UserId.ToString());
                var stockk = await stockRepository.GetByIdAsync(auction.StockUuid);

                await userStockService.CreateAsync(new UserStock
                {
                    StockCount = auction.InitialPrice / stockk.Price,
                    StockUuid = stockk.Uuid,
                    TotalPrice = auction.InitialPrice * (auction.InitialPrice / stockk.Price),
                    UserId = userAuctionn.Id
                });

                auction.EndTime = DateTime.Now;
                await auctionRepository.UpdateAsync(auction);

                return;
            }

            var userBidded = await userManager.FindByIdAsync(highestBid.UserId.ToString());
            var userAuction = await userManager.FindByIdAsync(auction.UserId.ToString());
            var stock = await stockRepository.GetByIdAsync(auction.StockUuid);

            var userStock = new UserStock
            {
                StockCount = auction.InitialPrice / stock.Price,
                StockUuid = stock.Uuid,
                TotalPrice = highestBid.BidAmount,
                UserId = userBidded.Id
            };

            await userStockRepository.CreateAsync(userStock);

            userBidded.StocksBalance += highestBid.BidAmount;
            userBidded.Balance -= highestBid.BidAmount;
            await userManager.UpdateAsync(userBidded);

            userAuction.Balance += highestBid.BidAmount;
            userAuction.StocksBalance -= auction.InitialPrice;
            await userManager.UpdateAsync(userAuction);

            auction.EndTime = DateTime.Now;
        }

        await auctionRepository.UpdateAsync(auction);
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
