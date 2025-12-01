namespace Auktion_API.DTOs;

public class BidDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public DateTime PlacedAt { get; set; }
    public int LotId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = "";
}