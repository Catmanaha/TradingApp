using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services
{
    public class UserStockService : IUserStockService
    {
        private readonly IStockRepository stockRepository;
        private readonly IUserStockRepository userStockRepository;
        private readonly UserManager<User> userManager;

        public UserStockService(
            IStockRepository stockRepository,
            IUserStockRepository userStockRepository,
            UserManager<User> userManager
        )
        {
            this.stockRepository = stockRepository;
            this.userStockRepository = userStockRepository;
            this.userManager = userManager;
        }

        public async Task<UserStock> CreateAsync(UserStock model)
        {
            var userStocks = await userStockRepository.GetAllAsync();
            var userStocksFiltered = userStocks.FirstOrDefault(o => o.UserId == model.UserId && o.StockUuid == model.StockUuid);

            if (userStocksFiltered is null)
            {
                await userStockRepository.CreateAsync(model);
            }
            else
            {
                userStocksFiltered.StockCount += model.StockCount;
                userStocksFiltered.TotalPrice += model.TotalPrice;

                await userStockRepository.UpdateAsync(userStocksFiltered);
            }

            return model;
        }

        public async Task<IEnumerable<UserStockForUser>> GetAllForUser(int id)
        {
            var query = from userStock in await userStockRepository.GetAllAsync()
                        join stock in await stockRepository.GetAllAsync() on userStock.StockUuid equals stock.Uuid
                        join user in await userManager.Users.Where(o => o.Id == id).ToListAsync() on userStock.UserId equals user.Id
                        select new UserStockForUser
                        {
                            StockUuid = stock.Uuid,
                            UserStockId = userStock.Id,
                            StockIconUrl = stock.IconUrl,
                            StockName = stock.Name,
                            UserStockTotalPrice = userStock.TotalPrice,
                            StockCount = userStock.StockCount

                        };

            return query;
        }
    }
}