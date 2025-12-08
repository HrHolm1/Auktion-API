using Auktion_API.DataAccess;
using Auktion_API.Interfaces;
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
            .Include(l => l.Images)
            .AsNoTracking()
            .ToListAsync();
    }
    
    // Get all lots from a specific auction by id
    public async Task<List<Lot>> GetAllFromAuctionAsync(int auctionId)
    {
        return await _db.Lots
            .Include(l => l.Images)
            .AsNoTracking()
            .Where(l => l.AuctionId == auctionId)
            .ToListAsync();
    }
    
    //Gets a lot by its ID
    public async Task<Lot?> GetByIdAsync(int id)
    {
        return await _db.Lots
            .Include(l => l.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    
    //Creates a lot
    public async Task<Lot> CreateAsync(Lot lot)
    {
        // Find næste lotnummer i denne auktion
        var maxLotNumber = await _db.Lots
            .Where(l => l.AuctionId == lot.AuctionId)
            .MaxAsync(l => (int?)l.LotNumber) ?? 0;

        lot.LotNumber = maxLotNumber + 1;
        
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

    public async Task<Lot?> GetLotByAuctionIdAsync(int auctionId, int lotNumber)
    {
        var lot = await _db.Lots
            .Include(l => l.Images)
            .Where(l => l.AuctionId == auctionId && l.LotNumber == lotNumber)
            .FirstOrDefaultAsync();

        return lot ?? null;
    }

    public async Task<bool> CloseLotAsync(int lotId)
    {
        var lot = await _db.Lots.FindAsync(lotId);
        if (lot == null)
            return false;

        if (lot.IsClosed)
            return true;

        var winningBid = await _db.Bids
            .Where(b => b.LotId == lotId)
            .OrderByDescending(b => b.Amount)
            .ThenByDescending(b => b.PlacedAt)
            .FirstOrDefaultAsync();

        if (winningBid != null)
        {
            lot.WinnerUserId = winningBid.UserId;
            lot.EndingPrice = winningBid.Amount;
        }

        lot.IsClosed = true;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<LotImage>?> AddImagesAsync(int lotId, List<IFormFile> files, string webRootPath)
    {
        var lot = await _db.Lots.FirstOrDefaultAsync(l => l.Id == lotId);
        if (lot == null)
            return null; // controller can turn this into 404

        if (files.Count == 0)
            return new List<LotImage>();

        var uploadPath = Path.Combine(webRootPath, "uploads", "lots", lotId.ToString());
        Directory.CreateDirectory(uploadPath);

        var createdImages = new List<LotImage>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var image = new LotImage
            {
                LotId = lotId,
                FileName = fileName,
                Url = $"http://localhost:5264/uploads/lots/{lotId}/{fileName}"
            };

            createdImages.Add(image);
        }

        _db.LotImages.AddRange(createdImages);
        await _db.SaveChangesAsync();

        return createdImages;
    }
}