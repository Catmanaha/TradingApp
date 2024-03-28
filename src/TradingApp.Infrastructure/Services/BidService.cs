using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Dtos;
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

    public async Task Bid(BidDto dto)
    {
        if (dto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }

        var highestBid = await bidRepository.GetHighestBidForAuction(dto.AuctionId);
        var auction = await auctionRepository.GetByIdAsync(dto.AuctionId);

        if (highestBid is null && dto.BidAmount < auction.InitialPrice)
        {
            throw new ArgumentException($"Bid should be more than the initial price '{auction.InitialPrice}'", nameof(auction.InitialPrice));
        }

        if (highestBid is not null && highestBid.BidAmount > dto.BidAmount)
        {
            throw new ArgumentException($"Bid should be more than the highest bid '{highestBid.BidAmount}'", nameof(highestBid.BidAmount));
        }
    }

    public async Task CreateAsync(BidDto dto, User user)
    {
        if (dto.AuctionId < 0)
        {
            throw new ArgumentException("AuctionId cannot be negative");
        }

        if (dto.BidAmount < 0)
        {
            throw new ArgumentException("BidAmount cannot be negative");
        }

        if (dto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }

        if (user is null)
        {
            throw new NullReferenceException("User cannot be null");
        }

        await bidRepository.CreateAsync(new Bid
        {
            AuctionId = dto.AuctionId,
            BidAmount = dto.BidAmount,
            BidTime = DateTime.Now,
            UserId = user.Id,
        });
    }

    public async Task<IEnumerable<BidForAuction>> GetAllForAuction(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id cannot be negative");
        }

        var query = from bid in (await bidRepository.GetAllAsync()).OrderByDescending(g => g.AuctionId)
                    join auction in await auctionRepository.GetAllByIdAsync(id) on bid.AuctionId equals auction.Id
                    join user in await userManager.Users.ToListAsync() on bid.UserId equals user.Id
                    select new BidForAuction
                    {
                        BidTime = bid.BidTime,
                        BidAmount = bid.BidAmount,
                        UserName = user.UserName,
                        AuctionId = bid.AuctionId
                    };

        if (query is null)
        {
            return Enumerable.Empty<BidForAuction>();
        }

        return query;
    }

}