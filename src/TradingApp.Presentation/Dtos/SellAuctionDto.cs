using TradingApp.Core.Enums;
using TradingApp.Core.Models;

namespace TradingApp.Presentation.Dtos;

public class SellAuctionDto
{
    public string StockUuid { get; set; }
    public int UserStockId { get; set; }
    public int UserId { get; set; }
    public double InitialPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuctionStatusEnum Status { get; set; }
    public double Count { get; set; }
}
