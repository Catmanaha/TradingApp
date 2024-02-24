using TradingApp.Core.Enums;

namespace TradingApp.Core.Models;

public class User
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Password { get; set; }
    public UserRolesEnum Role { get; set; }
}
