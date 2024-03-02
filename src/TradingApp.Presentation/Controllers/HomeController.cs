using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Enums;
using TradingApp.Core.Repositories;

namespace TradingApp.Presentation.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
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
