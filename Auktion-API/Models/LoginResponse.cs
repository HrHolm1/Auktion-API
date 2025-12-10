namespace Auktion_API.Models;

public class LoginResponse
{
    public string Token { get; set; }
    public string Role {get; set;}
    public int userId { get; set; }
    public string username { get; set; }
}