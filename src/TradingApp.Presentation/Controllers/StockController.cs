using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Models.Stocks;
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

        try
        {
            var stocks = await stockService.GetAllWithOffsetAsync(offset);

            return View(new StocksGetAllViewModel
            {
                Stocks = stocks,
                Offset = offset
            });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


    }

    public async Task<IActionResult> Get(string id)
    {

        try
        {
            var stock = await stockService.GetByIdAsync(id);
            return View(stock);

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


    }

    public async Task<IActionResult> GetPriceHistory(string id)
    {
        try
        {
            var stockPriceHistory = await stockService.GetStockPriceHistory(id);
            var json = JsonSerializer.Serialize(stockPriceHistory);

            return Content(json, "application/json");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


    }

    public async Task<IActionResult> GetOHCL(string id)
    {
        try
        {
            var stockOHCL = await stockService.GetStockOHLC(id);
            var json = JsonSerializer.Serialize(stockOHCL);

            return Content(json, "application/json");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }


    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}