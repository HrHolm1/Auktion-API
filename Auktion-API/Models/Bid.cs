namespace Auktion_API.Models;

public class Bid
{
    public int Id { get; set; }

    public int LotId { get; set; } //Foreign Key til Lot
    
    public string UserId { get; set; } = String.Empty; //User id måske hvis vi oprettet en user model

    public double Amount { get; set; }

    public DateTime PlacedAt { get; set; } //Datetime til at hvornår buddet er sat op, bliver til createdAt i DB.
    
    //public Lot Lot { get; set; } = null; // Navigation Property
}