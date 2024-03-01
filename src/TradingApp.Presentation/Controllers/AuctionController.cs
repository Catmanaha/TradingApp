using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Core.Enums;
using TradingApp.Presentation.ViewModels;
using TradingApp.Core.Services;
using TradingApp.Core.Dtos;
using TradingApp.Infrastructure.Services;

namespace TradingApp.Presentation.Controllers;

[Authorize]
public class AuctionController : Controller
{
    private readonly IAuctionService auctionService;
    private readonly IBidService bidService;
    private readonly IUserService userService;
    private readonly IStockService stockService;

    public AuctionController(
        IAuctionService auctionService,
        IBidService bidService,
        IUserService userService,
        IStockService stockService
    )
    {
        this.auctionService = auctionService;
        this.bidService = bidService;
        this.userService = userService;
        this.stockService = stockService;
    }

    public IActionResult Bid(int auctionId)
    {
        return View(auctionId);
    }

    [HttpPost]
    public async Task<IActionResult> Bid(BidDto dto)
    {
        await bidService.Bid(dto);

        var user = await userService.GetUser(User);

        await bidService.CreateAsync(dto, user);

        return RedirectToAction("Auction", new { id = dto.AuctionId });
    }

    public async Task<IActionResult> ChangeStatus(AuctionStatusEnum status, int id)
    {
        await auctionService.ChangeStatus(status, id);

        return RedirectToAction("Auction", new { id });
    }

    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var auctions = await auctionService.GetAllForView();

        return View(auctions);
    }

    public async Task<IActionResult> Auction(int id)
    {
        var auction = await auctionService.GetById(id);
        var bids = await bidService.GetAllForAuction(auction.Id);
        var auctionUser = await userService.GetById(auction.UserId);
        var currentUser = await userService.GetUser(User);
        var stockName = (await stockService.GetByIdAsync(auction.StockUuid)).Name;

        return View(new AuctionViewModel
        {
            Auction = auction,
            Bids = bids,
            AuctionUser = auctionUser,
            CurrentUser = currentUser,
            StockName = stockName
        });
    }

    public async Task<IActionResult> GetAllForUser()
    {
        var result = await auctionService.GetAllForUser(userService.GetId(User));

        return View(result);
    }

    public async Task<IActionResult> Sell(string stockUuid, int userStockId)
    {
        var userId = userService.GetId(User);
        var stock = await stockService.GetByIdAsync(stockUuid);

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

        await auctionService.Sell(dto);

        auction.InitialPrice *= dto.Count;

        await auctionService.CreateAsync(dto);

        return RedirectToAction("GetAllForUser");
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
