namespace TradingApp.Core.Models;

public class Bid
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public double BidAmount { get; set; }
    public DateTime BidTime { get; set; }
}
