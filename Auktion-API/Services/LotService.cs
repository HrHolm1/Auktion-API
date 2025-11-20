using Auktion_API.DataAccess;
using Auktion_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auktion_API.Services;

public class LotService : ILotService
{
    private readonly AuctionContext _db;

    public LotService(AuctionContext db)
    {
        _db = db;
    }
    
    //Gets all Lots
    public async Task<List<Lot>> GetAllAsync()
    {
        return await _db.Lots
            .AsNoTracking()
            .ToListAsync();
    }
    
    // Get all lots from a specific auction by id
    public async Task<List<Lot>> GetAllFromAuctionAsync(int auctionId)
    {
        return await _db.Lots
            .AsNoTracking()
            .Where(l => l.AuctionId == auctionId)
            .ToListAsync();
    }
    
    //Gets a lot by its ID
    public async Task<Lot?> GetByIdAsync(int id)
    {
        return await _db.Lots
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    
    //Creates a lot
    public async Task<Lot> CreateAsync(Lot lot)
    {
        
        // THE DATABASE SHOULD HANDLE THIS!!!!!!
        if (lot.LotNumber == 0 || lot.LotNumber.ToString() == "")
        {
            lot.LotNumber = await _db.Lots
                .Where(l => l.AuctionId == lot.AuctionId)
                .Select(l => (int?)l.LotNumber)
                .MaxAsync() + 1 ?? 1;
        }
        
        _db.Lots.Add(lot);
        await _db.SaveChangesAsync();
        
        return lot;
    }
    
    //Updates a lot
    public async Task<bool> UpdateAsync(Lot lot)
    {
        var exists = await _db.Lots.AnyAsync(l => l.Id == lot.Id);
        if (!exists)
            return false;

        _db.Lots.Update(lot);
        await _db.SaveChangesAsync();
        
        return true;
    }
    
    //Deletes a lot
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Lots.FindAsync(id);
        if (entity is null)
            return false;
        
        _db.Lots.Remove(entity);
        await _db.SaveChangesAsync();
        
        return true;
    }
    
}