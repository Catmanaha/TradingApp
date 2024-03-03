using Moq;
using TradingApp.Core.Dtos;
using TradingApp.Core.Models;
using TradingApp.Core.Repositories;
using TradingApp.Infrastructure.Services;

namespace TradingApp.UnitTest;

public class UserStockServiceTests
{
    [Fact]
    public async void Sell_DtoNull_ThrowNullReferenceException()
    {
        var service = new UserStockService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.Sell(null));
    }

    [Fact]
    public async void CreateAsync_DtoNull_ThrowNullReferenceException()
    {
        var service = new UserStockService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateAsync(null, new User()));
    }

    [Fact]
    public async void CreateAsync_UserNull_ThrowNullReferenceException()
    {
        var service = new UserStockService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.CreateAsync(new UserStockDto(), null));
    }

    [Fact]
    public async void CreateAsync_NewuserBalanceNegative_ThrowArgumentException()
    {
        var service = new UserStockService(null, null, null);

        var user = new User
        {
            Balance = 0
        };

        var dto = new UserStockDto
        {
            StockPrice = 1,
            StockCount = 1
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(dto, user));
    }

    [Fact]
    public async void GetAllForUser_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var service = new UserStockService(null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetAllForUser(id));
    }

    [Fact]
    public async void GetById_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var service = new UserStockService(null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetById(id));
    }

    [Fact]
    public async void GetById_UserStockNull_ThrowNullReferenceException()
    {
        var id = 1;
        var userStockRepo = new Mock<IUserStockRepository>();

        userStockRepo.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((UserStock?)null);

        var service = new UserStockService(null, userStockRepo.Object, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetById(id));
    }
}
