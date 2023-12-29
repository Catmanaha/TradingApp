namespace TradingApp.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }

    public User(int id, string email, string name, string surname, string password)
    {
        this.Id = id;
        this.Email = email;
        this.Name = name;
        this.Surname = surname;
        this.Password = password;
    }
}
