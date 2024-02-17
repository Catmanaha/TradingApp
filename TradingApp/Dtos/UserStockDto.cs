using System.ComponentModel.DataAnnotations;

namespace TradingApp.Dtos;

public class UserStockDto
{
    [Required(ErrorMessage = "UserId cannot be empty")]
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required(ErrorMessage = "StockId cannot be empty")]
    [Range(1, int.MaxValue)]
    public int StockId { get; set; }

    [Required(ErrorMessage = "StockName cannot be empty")]
    public string? StockName { get; set; }

    [Required(ErrorMessage = "StockCount cannot be empty")]
    [Range(1, int.MaxValue)]
    public int StockCount { get; set; }
}
