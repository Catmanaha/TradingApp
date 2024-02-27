namespace TradingApp.Core.Models.ReturnsForServices;

public class BidForAuction
{
    public int AuctionId { get; set; }
    public DateTime BidTime { get; set; }
    public double BidAmount { get; set; }
    public string UserName { get; set; }
}
