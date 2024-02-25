using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Presentation.Dtos;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

public class UserStockController : Controller
{
    private readonly IUserStockRepository repository;
    private readonly UserManager<User> userManager;

    public UserStockController(IUserStockRepository repository, UserManager<User> userManager)
    {
        this.repository = repository;
        this.userManager = userManager;
    }

    [Authorize]
    public IActionResult Create(string stockName, int stockId, double price)
    {
        return View(new UserStockViewModel
        {
            UserId = int.Parse(userManager.GetUserId(User)),
            Price = price,
            StockName = stockName,
            StockId = stockId
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
                StockId = userStockDto.StockId
            });
        }

        var totalPrice = userStockDto.StockPrice * userStockDto.StockCount;
        var user = await userManager.GetUserAsync(User);
        var newUserBalace = user.Balance - totalPrice;

        if (newUserBalace < 0)
        {
            ModelState.AddModelError("Balance", "You do not have enough money");
            return View(new UserStockViewModel
            {
                UserId = int.Parse(userManager.GetUserId(User)),
                Price = userStockDto.StockPrice,
                StockName = userStockDto.StockName,
                StockId = userStockDto.StockId
            });
        }

        user.StocksBalance += totalPrice;
        user.Balance = newUserBalace;
        await userManager.UpdateAsync(user);

        await repository.CreateAsync(new UserStock
        {
            UserId = userStockDto.UserId,
            StockId = userStockDto.StockId,
            StockCount = userStockDto.StockCount,
            TotalPrice = totalPrice
        });

        return RedirectToAction("GetAllForUser");
    }

    [Authorize]
    public async Task<IActionResult> GetAllForUser()
    {
        if (userManager.GetUserId(User) is null)
        {
            return RedirectToAction("Login", "User");
        }

        var stocks = await repository.GetAllForUser(int.Parse(userManager.GetUserId(User)));
        
        return View(stocks);
    }

    [Authorize]
    public IActionResult Sell(string stockName, int stockId)
    {
        return View(new SellUserStockViewModel
        {
            StockId = stockId,
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
                StockId = dto.StockId,
                StockName = dto.StockName
            });
        }

        var userStock = await repository.GetByIdAsync(dto.StockId);

        var totalCount = userStock.StockCount - dto.StockCount;

        if (totalCount < 0)
        {
            ModelState.AddModelError("Count", "You do not own that much stocks");
            return View(new SellUserStockViewModel
            {
                StockId = dto.StockId,
                StockName = dto.StockName
            });
        }

        var countBefore = userStock.StockCount;

        await repository.Sell(userStock, dto.StockCount);
        var user = await userManager.GetUserAsync(User);
        
        user.Balance += (userStock.TotalPrice / countBefore) * dto.StockCount;
        user.StocksBalance -= (userStock.TotalPrice / countBefore)  * dto.StockCount;
        await userManager.UpdateAsync(user);

        return RedirectToAction("Profile", "User");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
