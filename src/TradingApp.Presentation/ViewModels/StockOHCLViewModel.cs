using TradingApp.Core.Models;

namespace TradingApp.Presentation.ViewModels;

public class StockOHCLViewModel
{
    public IEnumerable<StockOHLC> OHCLs { get; set; }
    public Stock Stock { get; set; }
}
