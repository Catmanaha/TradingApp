using TradingApp.Core.Models;

namespace TradingApp.Presentation.ViewModels;

public class StocksViewModel
{
    public IEnumerable<Stock> Stocks { get; set; }
    public string ImagePath { get; set; }
}
