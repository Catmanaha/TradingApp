using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Dtos;
using TradingApp.Enums;
using TradingApp.Models;
using TradingApp.Repositories.Base;

namespace TradingApp.Controllers;

public class StockController : Controller
{
    private readonly IStockRepository repository;

    public StockController(IStockRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<IActionResult> GetAll()
    {
        var getAll = await repository.GetAllAsync();
        return View(getAll);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StockDto stock)
    {
        if (ModelState.IsValid == false) {
            return View();
        }

        await repository.CreateAsync(new Stock
        {
            MarketCap = stock.MarketCap,
            Symbol = stock.Symbol,
            Name = stock.Name
        });

        return RedirectToAction("GetAll");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
