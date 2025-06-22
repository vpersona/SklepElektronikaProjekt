namespace ProjektSklepElektronika.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User";

    public static List<User> Users = new List<User>();

}
