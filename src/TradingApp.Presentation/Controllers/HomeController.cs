using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Services;
using TradingApp.Presentation.ViewModels;

namespace TradingApp.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IStockService stockService;
    private readonly INewsService newsService;

    public HomeController(IStockService stockService, INewsService newsService)
    {
        this.stockService = stockService;
        this.newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        return View(new HomeViewModel{
            Articles = (await newsService.GetAll()).Articles.Take(3),
            Stocks = (await stockService.GetAll()).Take(5)
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
