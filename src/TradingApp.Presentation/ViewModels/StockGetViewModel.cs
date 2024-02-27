using TradingApp.Core.Models;

namespace TradingApp.Presentation.ViewModels;

public class StockGetViewModel
{
    public Stock Stock { get; set; }
    public IEnumerable<StockPriceHistory> StockPriceHistory { get; set; }
    public IEnumerable<StockOHLC> StockOHLC { get; set; }
}
