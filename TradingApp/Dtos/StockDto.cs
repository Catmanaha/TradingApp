using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Dtos;

public class StockDto
{
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public string? MarketCap { get; set; }
}
