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
    public IActionResult Create(string stockName, int stockId)
    {
        return View(new UserStockViewModel
        {
            UserId = int.Parse(userManager.GetUserId(User)),
            StockName = stockName,
            StockId = stockId
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserStockDto userStockDto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        await repository.CreateAsync(new UserStock
        {
            UserId = userStockDto.UserId,
            StockName = userStockDto.StockName,
            StockId = userStockDto.StockId,
            StockCount = userStockDto.StockCount
        });

        return RedirectToAction("GetAllForUser");
    }

    [Authorize]
    public IActionResult GetAllForUser()
    {
        if (userManager.GetUserId(User) is null)
        {
            return RedirectToAction("Login", "User");
        }
    
        var stocks = repository.GetAllForUser(int.Parse(userManager.GetUserId(User)));
        return View(stocks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
