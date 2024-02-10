using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Presentation.Dtos;

namespace TradingApp.Presentation.Controllers;

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
        var errors = new List<string>();

        if (long.IsNegative(stock.MarketCap))
        {
            errors.Add("Market capacity cannot be negative");
        }

        if (string.IsNullOrEmpty(stock.Name))
        {
            errors.Add("Dont leave name empty");
        }

        if (string.IsNullOrEmpty(stock.Symbol))
        {
            errors.Add("Dont leave symbol empty");
        }

        if (errors.Any())
        {
            return View(errors);
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
