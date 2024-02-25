using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}