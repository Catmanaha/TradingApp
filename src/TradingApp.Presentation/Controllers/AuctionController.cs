using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Core.Enums;
using TradingApp.Presentation.Dtos;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

public class AuctionController : Controller
{
    private readonly IAuctionRepository repository;
    private readonly UserManager<User> userManager;
    private readonly IUserStockRepository userStockRepository;
    private readonly IBidRepository bidRepository;
    private readonly IStockRepository stockRepository;

    public AuctionController(IAuctionRepository repository,
        UserManager<User> userManager,
        IUserStockRepository userStockRepository,
        IBidRepository bidRepository,
        IStockRepository stockRepository)
    {
        this.repository = repository;
        this.userManager = userManager;
        this.userStockRepository = userStockRepository;
        this.bidRepository = bidRepository;
        this.stockRepository = stockRepository;
    }

    [Authorize]
    public async Task<IActionResult> Bid(int auctionId)
    {
        return View(auctionId);
    }

    [HttpPost]
    public async Task<IActionResult> Bid(BidDto dto)
    {

        var highestBid = await bidRepository.GetHighestBidForAuction(dto.AuctionId);
        var auction = await repository.GetByIdAsync(dto.AuctionId);

        if (highestBid is null && dto.BidAmount < auction.InitialPrice)
        {
            ModelState.AddModelError("BidAmount", $"Bid should be more than the initial price '{auction.InitialPrice}'");
            return View(dto.AuctionId);
        }

        if (highestBid is not null && highestBid.BidAmount > dto.BidAmount)
        {
            ModelState.AddModelError("BidAmount", $"Bid should be more than the highest bid '{highestBid.BidAmount}'");
            return View(dto.AuctionId);
        }

        var user = await userManager.GetUserAsync(User);

        var bid = new Bid
        {
            AuctionId = dto.AuctionId,
            BidAmount = dto.BidAmount,
            BidTime = DateTime.Now,
            UserId = user.Id,
        };

        await bidRepository.CreateAsync(bid);

        return RedirectToAction("Auction", new { id = dto.AuctionId });
    }

    [Authorize]
    public async Task<IActionResult> ChangeStatus(AuctionStatusEnum status, int id)
    {
        var auction = await repository.GetByIdAsync(id);
        auction.Status = status;

        await repository.UpdateAsync(auction);

        if (status == AuctionStatusEnum.Closed)
        {
            var highestBid = await bidRepository.GetHighestBidForAuction(id);

            if (highestBid is null)
            {
                return RedirectToAction("Auction", new { id });
            }

            var userBidded = await userManager.FindByIdAsync(highestBid.UserId.ToString());
            var userAuction = await userManager.FindByIdAsync(auction.UserId.ToString());
            var stock = await stockRepository.GetByIdAsync(auction.StockId);

            var userStock = new UserStock
            {
                StockCount = highestBid.BidAmount / stock.Price,
                StockId = auction.StockId,
                TotalPrice = highestBid.BidAmount,
                UserId = userBidded.Id
            };

            await userStockRepository.CreateAsync(userStock);

            userBidded.StocksBalance += highestBid.BidAmount;
            await userManager.UpdateAsync(userBidded);

            userAuction.Balance += highestBid.BidAmount;
            await userManager.UpdateAsync(userAuction);
        }

        return RedirectToAction("Auction", new { id });
    }

    public async Task<IActionResult> GetAll()
    {
        var auctions = await repository.GetAllAsync();

        return View(auctions);
    }

    [Authorize]
    public async Task<IActionResult> Auction(int id)
    {
        var auction = await repository.GetByIdAsync(id);
        var bids = await bidRepository.GetAllForAuction(auction.Id);
        var auctionUser = await userManager.FindByIdAsync(auction.UserId.ToString());
        var currentUser = await userManager.GetUserAsync(User);
        var stockName = (await stockRepository.GetByIdAsync(auction.StockId)).Name;

        return View(new AuctionViewModel
        {
            Auction = auction,
            Bids = bids,
            AuctionUser = auctionUser,
            CurrentUser = currentUser,
            StockName = stockName
        });
    }

    [Authorize]
    public async Task<IActionResult> GetAllForUser()
    {
        var result = await repository.GetAllForUser(int.Parse(userManager.GetUserId(User)));

        return View(result);
    }

    [Authorize]
    public async Task<IActionResult> Sell(int stockId, int userStockId)
    {
        var userId = int.Parse(userManager.GetUserId(User));
        var stock = await stockRepository.GetByIdAsync(stockId);

        var auction = new Auction
        {
            StartTime = DateTime.Now,
            InitialPrice = stock.Price,
            EndTime = default,
            Status = AuctionStatusEnum.Open,
            UserId = userId,
            StockId = stockId
        };

        return View(new SellAuctionViewModel
        {
            Auction = auction,
            UserStockId = userStockId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Sell(SellAuctionDto dto)
    {
        var auction = new Auction
        {
            EndTime = dto.EndTime,
            InitialPrice = dto.InitialPrice,
            StartTime = dto.StartTime,
            UserId = dto.UserId,
            Status = dto.Status,
            StockId = dto.StockId
        };

        if (ModelState.IsValid == false)
        {
            return View(new SellAuctionViewModel
            {
                Auction = auction,
                UserStockId = dto.UserStockId
            });
        }

        var userStock = await userStockRepository.GetByIdAsync(dto.UserStockId);
        var totalCount = userStock.StockCount - dto.Count;

        if (totalCount < 0)
        {
            ModelState.AddModelError("Count", "U do not own that much stocks");
            return View(new SellAuctionViewModel
            {
                Auction = auction,
                UserStockId = dto.UserStockId
            });
        }

        userStock.StockCount -= dto.Count;
        await userStockRepository.UpdateAsync(userStock);

        auction.InitialPrice *= dto.Count;

        await repository.CreateAsync(auction);

        return RedirectToAction("GetAllForUser");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
