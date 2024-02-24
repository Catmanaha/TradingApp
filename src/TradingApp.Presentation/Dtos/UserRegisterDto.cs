using System.ComponentModel.DataAnnotations;

namespace TradingApp.Presentation.Dtos;

public class UserRegisterDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email cannot be empty")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Name cannot be empty")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Surname cannot be empty")]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Password cannot be empty")]
    public string? Password { get; set; }
}
