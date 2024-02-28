using TradingApp.Core.Models;

namespace TradingApp.Presentation.ViewModels;

public class StockPriceHistoryViewModel
{
    public IEnumerable<StockPriceHistory> PriceHistory { get; set; }
    public Stock Stock { get; set; }
}
