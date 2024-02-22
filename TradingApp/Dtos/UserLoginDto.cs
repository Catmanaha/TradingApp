using System.ComponentModel.DataAnnotations;

namespace TradingApp.Dtos;

public class UserLoginDto
{
    public string? ReturnUrl { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email cannot be empty")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password cannot be empty")]
    public string? Password { get; set; }
}
