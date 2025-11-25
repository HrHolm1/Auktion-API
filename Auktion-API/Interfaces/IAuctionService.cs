using Auktion_API.Models;

namespace Auktion_API.Interfaces;

public interface IAuctionService
{
    Task<List<Auction>> GetAllAsync();
    Task<Auction?> GetByIdAsync(int id);
    Task<Auction> CreateAsync(Auction auction);
    Task<bool> UpdateAsync(Auction auction);
    Task<bool> DeleteAsync(int id);
}