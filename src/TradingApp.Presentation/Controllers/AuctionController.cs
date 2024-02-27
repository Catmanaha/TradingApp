using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Core.Enums;
using TradingApp.Presentation.Dtos;
using TradingApp.Presentation.ViewModels;
using TradingApp.Core.Services;

namespace TradingApp.Presentation.Controllers;

public class AuctionController : Controller
{
    private readonly IAuctionRepository repository;
    private readonly UserManager<User> userManager;
    private readonly IUserStockRepository userStockRepository;
    private readonly IBidRepository bidRepository;
    private readonly IStockRepository stockRepository;
    private readonly IAuctionService auctionService;
    private readonly IBidService bidService;

    public AuctionController(IAuctionRepository repository,
        UserManager<User> userManager,
        IUserStockRepository userStockRepository,
        IBidRepository bidRepository,
        IStockRepository stockRepository,
        IAuctionService auctionService,
        IBidService bidService)
    {
        this.repository = repository;
        this.userManager = userManager;
        this.userStockRepository = userStockRepository;
        this.bidRepository = bidRepository;
        this.stockRepository = stockRepository;
        this.auctionService = auctionService;
        this.bidService = bidService;
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

        await bidRepository.CreateAsync(new Bid
        {
            AuctionId = dto.AuctionId,
            BidAmount = dto.BidAmount,
            BidTime = DateTime.Now,
            UserId = user.Id,
        });

        return RedirectToAction("Auction", new { id = dto.AuctionId });
    }

    [Authorize]
    public async Task<IActionResult> ChangeStatus(AuctionStatusEnum status, int id)
    {
        var auction = await repository.GetByIdAsync(id);
        auction.Status = status;


        if (status == AuctionStatusEnum.Closed)
        {
            var highestBid = await bidRepository.GetHighestBidForAuction(id);

            if (highestBid is null)
            {
                await repository.UpdateAsync(auction);
                return RedirectToAction("Auction", new { id });
            }

            var userBidded = await userManager.FindByIdAsync(highestBid.UserId.ToString());
            var userAuction = await userManager.FindByIdAsync(auction.UserId.ToString());
            var stock = await stockRepository.GetByIdAsync(auction.StockUuid);

            var userStock = new UserStock
            {
                StockCount = auction.InitialPrice / stock.Price,
                StockUuid = "s",
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
        }

        auction.EndTime = DateTime.Now;
        await repository.UpdateAsync(auction);

        return RedirectToAction("Auction", new { id });
    }

    public async Task<IActionResult> GetAll()
    {
        var auctions = await auctionService.GetAllForView();

        return View(auctions);
    }

    [Authorize]
    public async Task<IActionResult> Auction(int id)
    {
        var auction = await repository.GetByIdAsync(id);
        var bids = await bidService.GetAllForAuction(auction.Id);
        var auctionUser = await userManager.FindByIdAsync(auction.UserId.ToString());
        var currentUser = await userManager.GetUserAsync(User);
        var stockName = (await stockRepository.GetByIdAsync(auction.StockUuid)).Name;

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
        var result = await auctionService.GetAllForUser(int.Parse(userManager.GetUserId(User)));
        
        return View(result);
    }

    [Authorize]
    public async Task<IActionResult> Sell(string stockUuid, int userStockId)
    {
        var userId = int.Parse(userManager.GetUserId(User));
        var stock = await stockRepository.GetByIdAsync(stockUuid);

        var auction = new Auction
        {
            StartTime = DateTime.Now,
            InitialPrice = stock.Price,
            EndTime = default,
            Status = AuctionStatusEnum.Open,
            UserId = userId,
            StockUuid = stockUuid
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
            StockUuid = dto.StockUuid
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

        await userStockRepository.Sell(userStock, dto.Count);

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
