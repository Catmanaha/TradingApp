using Microsoft.AspNetCore.Mvc;
using TradingApp.Dtos;
using TradingApp.Models;
using TradingApp.Repositories.Base.Repositories;
using TradingApp.ViewModels;

namespace TradingApp.Controllers
{
    public class UserStockController : Controller
    {
        private readonly IUserStockRepository repository;

        public UserStockController(IUserStockRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Create(string stockName, int stockId)
        {
            return View(new UserStockViewModel
            {
                StockName = stockName,
                StockId = stockId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserStockDto userStockDto)
        {

            var errors = new List<string>();

            if (userStockDto.StockCount == 0)
            {
                errors.Add("Count cannot be zero");
            }

            if (userStockDto.StockCount < 0)
            {
                errors.Add("Count cannot be negative");
            }

            if (errors.Any()) {
                return View(new UserStockViewModel {
                    Errors = errors,
                    StockId = userStockDto.UserId,
                    StockName = userStockDto.StockName,
                });
            }


            await repository.CreateAsync(new UserStock
            {
                UserId = userStockDto.UserId,
                StockId = userStockDto.StockId,
                StockCount = userStockDto.StockCount
            });

            return RedirectToAction("Profile", "User");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}