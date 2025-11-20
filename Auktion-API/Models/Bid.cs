namespace Auktion_API.Models;

public class Bid
{
    public int Id { get; set; }

    public int LotNr { get; set; } //Ikke sikker på om det skal være LotNr eller LotId
    
    public string BidderName { get; set; } //User id måske hvis vi oprettet en user model

    public double Amount { get; set; }

    public DateTime PlacedAt { get; set; } //Datetime til at hvornår buddet er sat op, bliver til createdAt i DB.
    
    public List<Lot> Lots { get; set; } = new(); // Navigation Property

    
}