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
        var stocks = await repository.GetAllForViewAsync(offset);

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

        return View(stockPriceHistory);
    }

    public async Task<IActionResult> GetOHCL(string id)
    {
        var stockOHLC= await repository.GetStockOHLC(id);

        return View(stockOHLC);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}