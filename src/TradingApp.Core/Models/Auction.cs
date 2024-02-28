using TradingApp.Core.Enums;

namespace TradingApp.Core.Models;

public class Auction
{
    public int Id { get; set; }
    public string? StockUuid { get; set; }
    public int UserId { get; set; }
    public double InitialPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuctionStatusEnum Status { get; set; }
}
