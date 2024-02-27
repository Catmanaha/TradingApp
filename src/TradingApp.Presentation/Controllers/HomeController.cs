using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Enums;
using TradingApp.Core.Repositories;

namespace TradingApp.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IStockRepository repository;

    public HomeController(IStockRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
