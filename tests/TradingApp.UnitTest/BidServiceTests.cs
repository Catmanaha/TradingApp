namespace TradingApp.UnitTest;

public class BidServiceTests
{
    [Fact]
    public async void Bid_HighestBidNullAndBidAmountLessThanInitialPrice_ThrowArgumentException()
    {
        var bidRepoMock = new Mock<IBidRepository>();
        var auctionRepoMock = new Mock<IAuctionRepository>();

        var bidDto = new BidDto
        {
            AuctionId = 1,
            BidAmount = 5
        };

        bidRepoMock.Setup(repo => repo.GetHighestBidForAuction(bidDto.AuctionId)).ReturnsAsync((Bid?)null);
        auctionRepoMock.Setup(repo => repo.GetByIdAsync(bidDto.AuctionId)).ReturnsAsync(new Auction
        {
            InitialPrice = 10
        });

        var bidService = new BidService(bidRepoMock.Object, auctionRepoMock.Object, null);

        await Assert.ThrowsAsync<ArgumentException>(async () => await bidService.Bid(bidDto));
    }

    [Fact]
    public async void Bid_HighestBidNotNullAndBidAmountMoreThanDtoBidAmount_ThrowArgumentException()
    {
        var bidRepoMock = new Mock<IBidRepository>();
        var auctionRepoMock = new Mock<IAuctionRepository>();

        var bidDto = new BidDto
        {
            AuctionId = 1,
            BidAmount = 5
        };

        bidRepoMock.Setup(repo => repo.GetHighestBidForAuction(bidDto.AuctionId)).ReturnsAsync(new Bid
        {
            BidAmount = 10
        });

        var bidService = new BidService(bidRepoMock.Object, auctionRepoMock.Object, null);

        await Assert.ThrowsAsync<ArgumentException>(async () => await bidService.Bid(bidDto));
    }

    [Fact]
    public async void Bid_DtoNull_ThrowNullReferenceException()
    {
        var bidService = new BidService(null, null, null);

        await Assert.ThrowsAsync<NullReferenceException>(async () => await bidService.Bid(null));
    }

    [Fact]
    public async void GetAllForAuction_IdNegative_ThrowArgumentException()
    {
        var id = -1;
        var bidService = new BidService(null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(async () => await bidService.GetAllForAuction(id));
    }

    [Fact]
    public async void CreateAsync_ParametersIncorrect_ThrowExceptions()
    {
        var bidService = new BidService(null, null, null);

        await Assert.ThrowsAsync<ArgumentException>(async () => await bidService.CreateAsync(new BidDto
        {
            AuctionId = -1,
            BidAmount = 0
        }, new User()));

        await Assert.ThrowsAsync<ArgumentException>(async () => await bidService.CreateAsync(new BidDto
        {
            AuctionId = 0,
            BidAmount = -1
        }, new User()));

        await Assert.ThrowsAsync<NullReferenceException>(async () => await bidService.CreateAsync(null, new User()));
        await Assert.ThrowsAsync<NullReferenceException>(async () => await bidService.CreateAsync(new BidDto(), null));
    }

}
