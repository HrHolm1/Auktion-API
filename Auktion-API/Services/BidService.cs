using Auktion_API.DataAccess;
using Auktion_API.DTOs;
using Auktion_API.Hubs;
using Auktion_API.Interfaces;
using Auktion_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Auktion_API.Services;

public class BidService : IBidService
{
    private readonly AuctionContext _db;
    private readonly IHubContext<BidHub> _hubContext;
    
    public BidService(AuctionContext db,  IHubContext<BidHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }
    
    //Gets all bids for a specific lot
    public async Task<List<BidDto>> GetBidsByLotIdAsync(int lotId)
    {
        return await _db.Bids
            .AsNoTracking()
            .Where(b => b.LotId == lotId)
            .Include(b => b.User)
            .OrderByDescending(b => b.PlacedAt)
            .Select(b => new BidDto
            {
                Id = b.Id,
                Amount = b.Amount,
                PlacedAt = b.PlacedAt,
                LotId = b.LotId,
                UserId = b.UserId,
                Username = b.User.Email
            })
            .ToListAsync();
    }
    
    //Gets all bids placed by a specific user
    public async Task<List<Bid>> GetBidsByUserIdAsync(int userId)
    {
        return await _db.Bids
            .AsNoTracking()
            //.Include(b => b.Lot)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.PlacedAt)
            .ToListAsync();
    }
    
    //Places a new bid on a lot
    public async Task<Bid?> PlaceBidAsync(BidDto bidDtoInput)
    {

        var bidToPlace = new Bid
        {
            Id = bidDtoInput.Id,
            LotId = bidDtoInput.LotId,
            UserId = bidDtoInput.UserId,
            Amount = bidDtoInput.Amount,
            PlacedAt = bidDtoInput.PlacedAt,
        };
        
        var highestBid = await _db.Bids
            .Where(b => b.LotId == bidToPlace.LotId)   // only this lot
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();

        var lotToBidOn = await _db.Lots
            .Where(l => l.Id == bidToPlace.LotId)
            .FirstOrDefaultAsync();

        if (highestBid == null && bidToPlace.Amount < lotToBidOn.StartingPrice)
        {
            return null;
        }

        if (highestBid != null && bidToPlace.Amount <= highestBid.Amount) { 
            return null; 
        } 
        
        bidToPlace.PlacedAt = DateTime.UtcNow;

        _db.Bids.Add(bidToPlace);
        await _db.SaveChangesAsync();
        
        // Notify all clients watching this lot
        await _hubContext
            .Clients
            .Group(bidDtoInput.LotId.ToString())
            .SendAsync("NewBid", bidDtoInput);

        return bidToPlace;
    }

    
    //Gets the highest bid for a specific lot
    public async Task<Bid?> GetHighestBidForLotAsync(int lotId)
    {
        return await _db.Bids
            .AsNoTracking()
            .Where(b => b.LotId == lotId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();
    }
}