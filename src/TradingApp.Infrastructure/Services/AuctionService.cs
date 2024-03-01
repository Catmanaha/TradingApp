using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Dtos;
using TradingApp.Core.Enums;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository auctionRepository;
    private readonly UserManager<User> userManager;
    private readonly IBidRepository bidRepository;
    private readonly IUserStockService userStockService;
    private readonly IUserStockRepository userStockRepository;
    private readonly IStockService stockService;

    public AuctionService(
        IAuctionRepository auctionRepository,
        UserManager<User> userManager,
        IBidRepository bidRepository,
        IUserStockService userStockService,
        IUserStockRepository userStockRepository,
        IStockService stockService
    )
    {
        this.auctionRepository = auctionRepository;
        this.userManager = userManager;
        this.bidRepository = bidRepository;
        this.userStockService = userStockService;
        this.userStockRepository = userStockRepository;
        this.stockService = stockService;
    }

    public async Task<Auction> GetById(int id)
    {
        var auction = await auctionRepository.GetByIdAsync(id);

        if (auction is null)
        {
            throw new NullReferenceException("Auction not found");
        }

        return auction;
    }

    public async Task ChangeStatus(AuctionStatusEnum status, int id)
    {
        var auction = await GetById(id);
        auction.Status = status;


        if (status == AuctionStatusEnum.Closed)
        {
            var highestBid = await bidRepository.GetHighestBidForAuction(id);

            if (highestBid is null)
            {
                await auctionRepository.UpdateAsync(auction);

                var userAuctionn = await userManager.FindByIdAsync(auction.UserId.ToString());
                var stockk = await stockService.GetByIdAsync(auction.StockUuid);

                await userStockRepository.CreateAsync(new UserStock
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
            var stock = await stockService.GetByIdAsync(auction.StockUuid);

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
            var stock = await stockService.GetByIdAsync(auction.StockUuid);

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
                    join stock in await stockService.GetAll() on auction.StockUuid equals stock.Uuid
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

    public async Task Sell(SellAuctionDto dto)
    {
        var userStock = await userStockService.GetById(dto.UserStockId);
        var totalCount = userStock.StockCount - dto.Count;

        if (totalCount < 0)
        {
            throw new ArgumentException("U do not own that much stocks");
        }

        await userStockRepository.Sell(userStock, dto.Count);


    }

    public async Task<Auction> CreateAsync(SellAuctionDto dto)
    {
        var exceptions = new List<Exception>();

        if (dto.Count == 0) {
            exceptions.Add(new ArgumentException("Count cannot be 0"));
        }

        if (dto.Count < 0) {
            exceptions.Add(new ArgumentException("Count cannot be negative"));
        }

        if (dto.InitialPrice < 0) {
            exceptions.Add(new ArgumentException("InitialPrice cannot be negative"));
        }


        if (string.IsNullOrEmpty(dto.StockUuid)) {
            exceptions.Add(new ArgumentException("Stock uuid cannot be empty"));
        }

        if (exceptions.Any()) {
            throw new AggregateException(exceptions);
        }

        var auction = await auctionRepository.CreateAsync(new Auction
        {
            InitialPrice = dto.InitialPrice,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Status = dto.Status,
            StockUuid = dto.StockUuid,
            UserId = dto.UserId
        });

        return auction;
    }
}
