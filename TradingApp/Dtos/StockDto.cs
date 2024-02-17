using System.ComponentModel.DataAnnotations;

namespace TradingApp.Dtos;

public class StockDto
{
    [Required(ErrorMessage="Symbol cannot be empty")]
    public string? Symbol { get; set; }
    [Required(ErrorMessage="Name cannot be empty")]
    public string? Name { get; set; }
    [Required(ErrorMessage="Market Capacity cannot be empty")]
    [Range(0, long.MaxValue, ErrorMessage="Market Capacity cannot be negative")]
    public long MarketCap { get; set; }
}
