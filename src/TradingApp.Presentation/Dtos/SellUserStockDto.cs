using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class SellUserStockDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Count cannot be negative")]
    public int StockCount { get; set; }
    public int StockId { get; set; }
    public string StockName { get; set; }
}
