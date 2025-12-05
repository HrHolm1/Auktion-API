using Auktion_API.DataAccess;
using Auktion_API.Interfaces;
using Auktion_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auktion_API.Services;

public class UserService : IUserService
{
    private readonly AuctionContext _db;

    public UserService(AuctionContext db)
    {
        _db = db;
    }
    
    //Gets all users and their bids
    public async Task<List<User>> GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.Bids)
            .ToListAsync();
    }

    //Gets a user by their id, includes bids
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users
            .AsNoTracking()
            .Include(u => u.Bids)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    //Creates a new User
    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;
    }
    
    //Updates a User
    public async Task<bool> UpdateAsync(User user)
    {
        var exists = await _db.Users.AnyAsync(u => u.Id == user.Id);
        if (!exists)
            return false;

        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return true;
    }
    
    //Deletes a User
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Users.FindAsync(id);
        if (entity is null)
            return false;

        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Lot>> GetWonLotsAsync(int userId)
    {
        return await _db.Lots
            .Where(l => l.WinnerUserId == userId)
            .ToListAsync();
    }
}