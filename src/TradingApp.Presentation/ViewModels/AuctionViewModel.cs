using TradingApp.Core.Models;

namespace TradingApp.Presentation.ViewModels;

public class AuctionViewModel
{
    public Auction Auction { get; set; }
    public IEnumerable<dynamic> Bids { get; set; }
    public User AuctionUser { get; set; }
    public User CurrentUser { get; set; }
    public string StockName { get; set; }
}
