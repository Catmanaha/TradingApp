using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class StockDto
{
    [Required(ErrorMessage = "Symbol cannot be empty")]
    public string? Symbol { get; set; }

    [Required(ErrorMessage = "Name cannot be empty")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Market Capacity cannot be empty")]
    [Range(0, long.MaxValue, ErrorMessage = "Market Capacity cannot be negative")]
    public long MarketCap { get; set; }

    [Required(ErrorMessage = "Price cannot be empty")]
    [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
    public double Price { get; set; }

    public IFormFile? StockImage { get; set; }
}
