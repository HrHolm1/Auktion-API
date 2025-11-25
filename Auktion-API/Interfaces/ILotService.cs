using Auktion_API.Models;

namespace Auktion_API.Interfaces;

public interface ILotService
{
    Task<List<Lot>> GetAllAsync();
    Task<Lot?> GetByIdAsync(int id);
    Task<Lot> CreateAsync(Lot lot);
    Task<bool> UpdateAsync(Lot lot);
    Task<bool> DeleteAsync(int id);
    Task<Lot?> GetLotByAuctionIdAsync(int auctionId, int lotNumber);
}