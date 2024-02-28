using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Presentation.Dtos;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

public class StockController : Controller
{
    private readonly IStockRepository repository;

    public StockController(IStockRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> GetAll(int offset = 0)
    {
        var stocks = await repository.GetAllWithOffsetAsync(offset);

        return View(new StocksGetAllViewModel
        {
            Stocks = stocks,
            Offset = offset
        });
    }

    public async Task<IActionResult> Get(string id)
    {
        var stock = await repository.GetByIdAsync(id);

        return View(stock);
    }

    public async Task<IActionResult> GetPriceHistory(string id)
    {
        var stockPriceHistory = await repository.GetStockPriceHistory(id);
        var json = JsonSerializer.Serialize(stockPriceHistory);

        return Content(json, "application/json");
    }

    public async Task<IActionResult> GetOHCL(string id)
    {
        var stockPriceHistory = await repository.GetStockOHLC(id);
        var json = JsonSerializer.Serialize(stockPriceHistory);

        return Content(json, "application/json");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}