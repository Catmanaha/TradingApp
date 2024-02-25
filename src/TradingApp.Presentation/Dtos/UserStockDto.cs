using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class UserStockDto
{
    public int UserId { get; set; }

    public int StockId { get; set; }

    [Required(ErrorMessage = "StockCount cannot be empty")]
    [Range(1, int.MaxValue)]
    public int StockCount { get; set; }

    public double StockPrice { get; set; }
    public string StockName { get; set; }
}
