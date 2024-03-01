using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

public class StockController : Controller
{
    private readonly IStockService stockService;

    public StockController(IStockService stockService)
    {
        this.stockService = stockService;
    }

    public async Task<IActionResult> GetAll(int offset = 0)
    {
        var stocks = await stockService.GetAllWithOffsetAsync(offset);

        return View(new StocksGetAllViewModel
        {
            Stocks = stocks,
            Offset = offset
        });
    }

    public async Task<IActionResult> Get(string id)
    {
        var stock = await stockService.GetByIdAsync(id);

        return View(stock);
    }

    public async Task<IActionResult> GetPriceHistory(string id)
    {
        var stockPriceHistory = await stockService.GetStockPriceHistory(id);
        var json = JsonSerializer.Serialize(stockPriceHistory);

        return Content(json, "application/json");
    }

    public async Task<IActionResult> GetOHCL(string id)
    {
        var stockPriceHistory = await stockService.GetStockOHLC(id);
        var json = JsonSerializer.Serialize(stockPriceHistory);

        return Content(json, "application/json");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}