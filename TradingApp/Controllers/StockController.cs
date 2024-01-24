using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradingApp.Dtos;
using TradingApp.Models;
using TradingApp.Repositories;
using TradingApp.Repositories.Base;


namespace TradingApp.Controllers;

public class StockController : Controller
{
    private readonly ISqlRepository<Stock> repository;

    public StockController(ISqlRepository<Stock> repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> GetAll()
    {

        var getAll = await repository.GetAllAsync();

        return View(getAll);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StockDto stock)
    {

        if (string.IsNullOrEmpty(stock.MarketCap))
        {
            return BadRequest("Dont leave the market capacity field empty");
        }

        if (string.IsNullOrEmpty(stock.Name))
        {
            return BadRequest("Dont leave the name field empty");
        }

        if (string.IsNullOrEmpty(stock.Symbol))
        {
            return BadRequest("Dont leave the symbol field empty");
        }

        await repository.CreateAsync(new Stock
        {
            MarketCap = stock.MarketCap,
            Symbol = stock.Symbol,
            Name = stock.Name
        });

        return RedirectToAction("GetAll");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
