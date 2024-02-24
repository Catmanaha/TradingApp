using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Presentation.Dtos;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

public class UserStockController : Controller
{
    private readonly IUserStockRepository repository;

    public UserStockController(IUserStockRepository repository)
    {
        this.repository = repository;
    }

    [Authorize]
    public IActionResult Create(string stockName, int stockId)
    {
        return View(new UserStockViewModel
        {
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
            StockId = userStockDto.StockId,
            StockCount = userStockDto.StockCount
        });

        return RedirectToAction("GetAllForUser");
    }

    [Authorize]
    public async Task<IActionResult> GetAllForUser()
    {
        if (User.FindFirst("UserId") is null)
        {
            return RedirectToAction("Login", "User");
        }

        var stocks = await repository.GetAllForUserAsync(int.Parse(User.FindFirst("UserId").Value));
        return View(stocks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
