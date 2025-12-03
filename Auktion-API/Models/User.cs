namespace Auktion_API.Models;

public class User
{
    public int Id { get; set; } // Primary Key

    public string Email { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty; // Bliver gemt i clear text, men det går til dette
    
    public string Role { get; set; }
    
    public List<Bid> Bids { get; set; } = new(); // Navigation property
    
}