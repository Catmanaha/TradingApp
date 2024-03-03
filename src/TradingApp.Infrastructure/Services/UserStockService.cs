using Microsoft.AspNetCore.Identity;
using TradingApp.Core.Dtos;
using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;
using TradingApp.Core.Repositories;
using TradingApp.Core.Services;

namespace TradingApp.Infrastructure.Services;

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


    public async Task Sell(SellUserStockDto dto)
    {
        if (dto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }
        
        var userStock = await userStockRepository.GetByIdAsync(dto.UserStockId);

        var totalCount = userStock.StockCount - dto.StockCount;

        if (totalCount < 0)
        {
            throw new ArgumentException("You do not own that much stocks");
        }

        var countBefore = userStock.StockCount;

        await userStockRepository.Sell(userStock, dto.StockCount);
        var user = await userManager.FindByIdAsync(userStock.UserId.ToString());

        user.Balance += (userStock.TotalPrice / countBefore) * dto.StockCount;
        user.StocksBalance -= (userStock.TotalPrice / countBefore) * dto.StockCount;

        await userManager.UpdateAsync(user);
    }

    public async Task<UserStock> CreateAsync(UserStockDto userStockDto, User user)
    {
        if (userStockDto is null)
        {
            throw new NullReferenceException("Dto cannot be null");
        }

        if (user is null)
        {
            throw new NullReferenceException("User cannot be null");
        }

        var totalPrice = userStockDto.StockPrice * userStockDto.StockCount;

        var newUserBalace = user.Balance - totalPrice;

        if (newUserBalace < 0)
        {
            throw new ArgumentException("You do not have enough money");
        }

        user.StocksBalance += totalPrice;
        user.Balance = newUserBalace;
        await userManager.UpdateAsync(user);

        var userStock = new UserStock
        {
            StockCount = userStockDto.StockCount,
            StockUuid = userStockDto.StockUuid,
            TotalPrice = totalPrice,
            UserId = userStockDto.UserId
        };

        var userStocks = await userStockRepository.GetAllAsync();
        var userStocksFiltered = userStocks.FirstOrDefault(o => o.UserId == userStock.UserId && o.StockUuid == userStock.StockUuid);

        if (userStocksFiltered is null)
        {
            await userStockRepository.CreateAsync(userStock);
        }
        else
        {
            userStocksFiltered.StockCount += userStock.StockCount;
            userStocksFiltered.TotalPrice += userStock.TotalPrice;

            await userStockRepository.UpdateAsync(userStocksFiltered);
        }

        return userStock;
    }

    public async Task<IEnumerable<UserStockForUser>> GetAllForUser(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id cannot be neagative");
        }

        var userStocks = await userStockRepository.GetAllForUserAsync(id);
        var userStockForUsers = new List<UserStockForUser>();

        foreach (var userStock in userStocks)
        {
            var stock = await stockRepository.GetByIdAsync(userStock.StockUuid);
            userStockForUsers.Add(new UserStockForUser
            {
                StockUuid = stock.Uuid,
                UserStockId = userStock.Id,
                StockIconUrl = stock.IconUrl,
                StockName = stock.Name,
                UserStockTotalPrice = userStock.TotalPrice,
                StockCount = userStock.StockCount
            });
        }

        return userStockForUsers;
    }

    public async Task<UserStock> GetById(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id cannot be neagative");
        }

        var userStock = await userStockRepository.GetByIdAsync(id);

        if (userStock is null)
        {
            throw new NullReferenceException("UserStock not found");
        }

        return userStock;
    }
}
