using Auktion_API.DataAccess;
using Auktion_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Auktion_API.Services;

public class BidService : IBidService
{
    private readonly AuctionContext _db;

    public BidService(AuctionContext db)
    {
        _db = db;
    }
    
    //Gets all bids for a specific lot
    public async Task<List<Bid>> GetBidsByLotIdAsync(int lotId)
    {
        return await _db.Bids
            .AsNoTracking()
            .Include(b => b.Lot)
            .Where(b => b.LotId == lotId)
            .OrderByDescending(b => b.PlacedAt)
            .ToListAsync();
    }
    
    //Gets all bids placed by a specific user
    public async Task<List<Bid>> GetBidsByUserIdAsync(string userId)
    {
        return await _db.Bids
            .AsNoTracking()
            .Include(b => b.Lot)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.PlacedAt)
            .ToListAsync();
    }
    
    //Places a new bid on a lot
    public async Task<Bid> PlaceBidAsync(Bid bid)
    {
        bid.PlacedAt = DateTime.UtcNow;

        _db.Bids.Add(bid);
        await _db.SaveChangesAsync();

        return bid;
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