using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "Admins")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StockDto stock)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }


        var newStock = await repository.CreateAsync(new Stock
        {
            MarketCap = stock.MarketCap,
            Symbol = stock.Symbol,
            Name = stock.Name,
        });

        var fileExtension = new FileInfo(stock.StockImage.FileName).Extension;
        var fileName = $"{newStock.Id}{fileExtension}";
        var destination = $"../../Assets/StockImages/{fileName}";

        newStock.ImageUrl = fileName;
        await repository.UpdateAsync(newStock);

        using var fileStream = System.IO.File.Create(destination);
        await stock.StockImage.CopyToAsync(fileStream);

        return RedirectToAction("GetAll");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}