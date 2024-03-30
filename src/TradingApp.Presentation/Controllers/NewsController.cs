using Microsoft.AspNetCore.Mvc;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Presentation.Controllers;

public class NewsController : Controller
{
    private readonly INewsService newsService;

    public NewsController(INewsService newsService)
    {
        this.newsService = newsService;
    }

    public async Task<IActionResult> GetAll(int offset = 0)
    {
        return View(await newsService.GetAll(offset));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
