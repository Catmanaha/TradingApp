using TradingApp.Core.Models.Stocks;

namespace TradingApp.Presentation.ViewModels;

public class StocksGetAllViewModel
{
    public IEnumerable<Stock>? Stocks { get; set; }
    public int Offset { get; set; }

}
