using Auktion_API.DataAccess;
using Auktion_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auktion_API.Services;

public class AuctionService : IAuctionService
{
    private readonly AuctionContext _db;

    public AuctionService(AuctionContext db)
    {
        _db = db;
    }

    // Gets all auctions
    public async Task<List<Auction>> GetAllAsync()
    {
        return await _db.Auctions
            .AsNoTracking()
            .Include(a => a.Lots)
            .ToListAsync();
    }
    
    //Gets an Auctions by its ID
    public async Task<Auction?> GetByIdAsync(int id)
    {
        return await _db.Auctions
            .AsNoTracking()
            .Include(a => a.Lots)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    //Creates an auction
    public async Task<Auction> CreateAsync(Auction auction)
    {
        _db.Auctions.Add(auction);
        await _db.SaveChangesAsync();
        return auction;
    }

    //Updates an auction
    public async Task<bool> UpdateAsync(Auction auction)
    {
        var exists = await _db.Auctions.AnyAsync(a => a.Id == auction.Id);
        if (!exists) return false;

        _db.Auctions.Update(auction);
        await _db.SaveChangesAsync();
        return true;
    }

    //Deletes an auction
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Auctions.FindAsync(id);
        if (entity is null) return false;

        _db.Auctions.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}