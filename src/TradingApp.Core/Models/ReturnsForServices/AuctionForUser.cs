using TradingApp.Core.Enums;

namespace TradingApp.Core.Models.ReturnsForServices;

public class AuctionForUser
{
    public int AuctionId { get; set; }
    public string StockName { get; set; }
    public double AuctionInitialPrice { get; set; }
    public string UserName { get; set; }
    public AuctionStatusEnum AuctionStatus { get; set; }
    public DateTime AuctionStartTime { get; set; }
    public DateTime AuctionEndTime { get; set; }
}
