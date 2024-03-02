using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Enums;
using TradingApp.Presentation.ViewModels;
using TradingApp.Core.Services;
using TradingApp.Core.Dtos;
using TradingApp.Core.Models.ReturnsForServices;

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
        try
        {
            await bidService.Bid(dto);
            var user = await userService.GetUser(User);

            await bidService.CreateAsync(dto, user);
            return RedirectToAction("Auction", new { id = dto.AuctionId });

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    public async Task<IActionResult> ChangeStatus(AuctionStatusEnum status, int id)
    {
        try
        {
            await auctionService.ChangeStatus(status, id);
            return RedirectToAction("Auction", new { id });

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var auctions = await auctionService.GetAllForView();

            return View(auctions);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    public async Task<IActionResult> Auction(int id)
    {
        try
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
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }
    }

    public async Task<IActionResult> GetAllForUser()
    {
        try
        {
            var result = await auctionService.GetAllForUser(userService.GetId(User));

            return View(result);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    public async Task<IActionResult> Sell(string stockUuid, int userStockId)
    {

        try
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
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

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

        try
        {
            await auctionService.Sell(dto);

            auction.InitialPrice *= dto.Count;

            await auctionService.CreateAsync(dto);

            return RedirectToAction("GetAllForUser");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
