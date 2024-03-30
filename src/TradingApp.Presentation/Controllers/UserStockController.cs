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
    private readonly IUserService userService;

    public UserStockController(
        UserManager<User> userManager,
        IUserStockService userStockService,
        IUserService userService
    )
    {
        this.userManager = userManager;
        this.userStockService = userStockService;
        this.userService = userService;
    }

    public IActionResult Create(string stockName, string stockUuid, double price)
    {
        return View(new UserStockViewModel
        {
            UserId = userService.GetId(User),
            Price = price,
            StockName = stockName,
            StockUuid = stockUuid
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserStockDto userStockDto)
    {

        if (!ModelState.IsValid)
        {
            return View(new UserStockViewModel
            {
                UserId = userService.GetId(User),
                Price = userStockDto.StockPrice,
                StockName = userStockDto.StockName,
                StockUuid = userStockDto.StockUuid
            });
        }

        var user = await userManager.GetUserAsync(User);

        var result = await userStockService.CreateAsync(userStockDto, user);

        if (!string.IsNullOrEmpty(result))
        {
            ModelState.AddModelError("Error", result);
            return View(new UserStockViewModel
            {
                UserId = userService.GetId(User),
                Price = userStockDto.StockPrice,
                StockName = userStockDto.StockName,
                StockUuid = userStockDto.StockUuid
            });
        }

        return RedirectToAction("GetAllForUser");
    }

    public async Task<IActionResult> GetAllForUser()
    {
        var id = userService.GetId(User);
        var stocks = await userStockService.GetAllForUser(id);

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

        var result = await userStockService.Sell(dto);

        if (!string.IsNullOrEmpty(result))
        {
            ModelState.AddModelError("Error", result);
            return View(new SellUserStockViewModel
            {
                UserStockId = dto.UserStockId,
                StockName = dto.StockName
            });
        }

        return RedirectToAction("Profile", "User");
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
