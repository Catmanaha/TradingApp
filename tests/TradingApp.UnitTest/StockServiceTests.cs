using TradingApp.Core.Models.Stocks;

namespace TradingApp.UnitTest;

public class StockServiceTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async void GetStockOHLC_SendIncorrectId_ThrowNulReferenceExecption(string id)
    {
        var stockRepoMock = new Mock<IStockRepository>();

        var service = new StockService(stockRepoMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetStockOHLC(id));
    }

    [Fact]
    public async void GetStockOHLC_StocksNull_ReturnEmptyCollection()
    {
        var id = "id";
        var stockRepoMock = new Mock<IStockRepository>();

        stockRepoMock.Setup(repo => repo.GetStockOHLC(id)).ReturnsAsync((IEnumerable<StockOHLC>?)null);

        var service = new StockService(stockRepoMock.Object);

        var stocks = await service.GetStockOHLC(id);

        Assert.Equal(Enumerable.Empty<StockOHLC>(), stocks);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("", "   ")]
    [InlineData("   ", "   ")]
    public async void GetStockPriceHistory_SendIncorrectIdAndTimeStamp_ThrowNulReferenceExecption(string id, string timestamp)
    {
        var stockRepoMock = new Mock<IStockRepository>();

        var service = new StockService(stockRepoMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetStockPriceHistory(id, timestamp));
    }

    [Fact]
    public async void GetStockPriceHistory_StocksNull_ReturnEmptyCollection()
    {
        var id = "id";
        var timestamp = "timestamp";
        var stockRepoMock = new Mock<IStockRepository>();

        stockRepoMock.Setup(repo => repo.GetStockPriceHistory(id, timestamp)).ReturnsAsync((IEnumerable<StockPriceHistory>?)null);

        var service = new StockService(stockRepoMock.Object);

        var stocks = await service.GetStockPriceHistory(id, timestamp);

        Assert.Equal(Enumerable.Empty<StockPriceHistory>(), stocks);

    }

    [Fact]
    public async void GetAll_StocksNull_ReturnEmptyCollection()
    {
        var stockRepoMock = new Mock<IStockRepository>();

        stockRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync((IEnumerable<Stock>?)null);

        var service = new StockService(stockRepoMock.Object);

        var stocks = await service.GetAll();

        Assert.Equal(Enumerable.Empty<Stock>(), stocks);
    }

    [Fact]
    public async void GetAllWithOffsetAsync_OffsetNegative_ThrowArgumentExecption()
    {
        var offset = -2;
        var stockRepoMock = new Mock<IStockRepository>();

        var service = new StockService(stockRepoMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetAllWithOffsetAsync(offset));
    }

    [Fact]
    public async void GetAllWithOffsetAsync_StockNull_ReturnEmptyCollection()
    {
        var offset = 1;
        var stockRepoMock = new Mock<IStockRepository>();

        stockRepoMock.Setup(repo => repo.GetAllWithOffsetAsync(offset)).ReturnsAsync((IEnumerable<Stock>?)null);

        var service = new StockService(stockRepoMock.Object);

        var stocks = await service.GetAllWithOffsetAsync(offset);

        Assert.Equal(Enumerable.Empty<Stock>(), stocks);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async void GetByIdAsync_SendIncorrectId_ThrowArgumentExecption(string id)
    {
        var stockRepoMock = new Mock<IStockRepository>();

        var service = new StockService(stockRepoMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(id));
    }

    [Fact]
    public async void GetByIdAsync_StockNull_ThrowNulReferenceExecption()
    {
        var id = "id";
        var stockRepoMock = new Mock<IStockRepository>();

        stockRepoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Stock?)null);

        var service = new StockService(stockRepoMock.Object);


        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetByIdAsync(id));
    }
}
