using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Services;
using TradingApp.Core.Dtos;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

[Authorize]
public class UserStockController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly IUserStockService userStockService;

    public UserStockController(
        UserManager<User> userManager,
        IUserStockService userStockService
    )
    {
        this.userManager = userManager;
        this.userStockService = userStockService;
    }

    public IActionResult Create(string stockName, string stockUuid, double price)
    {
        return View(new UserStockViewModel
        {
            UserId = int.Parse(userManager.GetUserId(User)),
            Price = price,
            StockName = stockName,
            StockUuid = stockUuid
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserStockDto userStockDto)
    {
        if (ModelState.IsValid == false)
        {
            return View(new UserStockViewModel
            {
                UserId = int.Parse(userManager.GetUserId(User)),
                Price = userStockDto.StockPrice,
                StockName = userStockDto.StockName,
                StockUuid = userStockDto.StockUuid
            });
        }

        var user = await userManager.GetUserAsync(User);

        await userStockService.CreateAsync(userStockDto, user);

        return RedirectToAction("GetAllForUser");
    }

    public async Task<IActionResult> GetAllForUser()
    {
        if (userManager.GetUserId(User) is null)
        {
            return RedirectToAction("Login", "User");
        }

        var stocks = await userStockService.GetAllForUser(int.Parse(userManager.GetUserId(User)));

        return View(stocks);
    }

    public IActionResult Sell(string stockName, int userStockId)
    {
        return View(new SellUserStockViewModel
        {
            UserStockId = userStockId,
            StockName = stockName
        });
    }

    [HttpPost]
    public async Task<IActionResult> Sell(SellUserStockDto dto)
    {

        if (!ModelState.IsValid)
        {
            return View(new SellUserStockViewModel
            {
                UserStockId = dto.UserStockId,
                StockName = dto.StockName
            });
        }

        await userStockService.Sell(dto);

        return RedirectToAction("Profile", "User");
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
