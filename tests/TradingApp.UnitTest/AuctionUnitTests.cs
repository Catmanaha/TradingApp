namespace TradingApp.UnitTest;

public class AuctionUnitTests
{
    [Fact]
    public async void GetAllForUser_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var auctionService = new AuctionService(null, null, null, null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(() => auctionService.GetAllForUser(id));
    }

    [Fact]
    public async void GetById_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var auctionService = new AuctionService(null, null, null, null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(() => auctionService.GetById(id));
    }

    [Fact]
    public async void GetById_AuctionNull_ThrowNullReferenceException()
    {
        var id = 1;
        var auctionRepoMock = new Mock<IAuctionRepository>();

        auctionRepoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Auction?)null);

        var auctionService = new AuctionService(null, null, null, null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => auctionService.GetById(id));
    }

    [Fact]
    public async void ChangeStatus_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var auctionService = new AuctionService(null, null, null, null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(() => auctionService.ChangeStatus(default, id));
    }
}
