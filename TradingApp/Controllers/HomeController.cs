using Microsoft.AspNetCore.Mvc;
using TradingApp.Repositories.Base;

namespace TradingApp.Controllers;

public class HomeController : Controller
{
    private readonly IStockRepository repository;

    public HomeController(IStockRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index() { 
        return View(await repository.GetRecentStocks());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
