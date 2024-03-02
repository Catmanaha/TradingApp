using System.ComponentModel.DataAnnotations;

namespace TradingApp.Core.Dtos;

public class UserRegisterDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email cannot be empty")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Username cannot be empty")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password cannot be empty")]
    public string? Password { get; set; }
}
