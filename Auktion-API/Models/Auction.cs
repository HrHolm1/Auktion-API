namespace Auktion_API.Models;

public class Auction
{
    public int Id { get; set; }
    
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public List<Lot> Lots { get; set; } = new(); // Navigation Property
}