using TradingApp.Core.Models;
using TradingApp.Core.Models.ReturnsForServices;

namespace TradingApp.Presentation.ViewModels;

public class AuctionViewModel
{
    public Auction Auction { get; set; }
    public IEnumerable<BidForAuction> Bids { get; set; }
    public User AuctionUser { get; set; }
    public User CurrentUser { get; set; }
    public string StockName { get; set; }
}
