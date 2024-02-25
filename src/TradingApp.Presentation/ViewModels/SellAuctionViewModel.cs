using TradingApp.Core.Models;

namespace TradingApp.Presentation.ViewModels;

public class SellAuctionViewModel
{
    public Auction Auction { get; set; }
    public int UserStockId { get; set; }
}
