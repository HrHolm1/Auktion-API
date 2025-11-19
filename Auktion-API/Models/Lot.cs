namespace Auktion_API.Models;

public class Lot
{
    public int Id { get; set; }
    public int LotNumber { get; set; }

    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }

    public double StartingPrice { get; set; }
    public double EndingPrice { get; set; }
    public double EstimatedPrice { get; set; } // Estimated price from the auction house

    public int AuctionId { get; set; }
}