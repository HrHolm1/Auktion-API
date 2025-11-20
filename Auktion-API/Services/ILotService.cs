using Auktion_API.Models;

namespace Auktion_API.Services;

public interface ILotService
{
    Task<List<Lot>> GetAllAsync();
    Task<Lot?> GetByIdAsync(int id);
    Task<Lot> CreateAsync(Lot lot);
    Task<bool> UpdateAsync(Lot lot);
    Task<bool> DeleteAsync(int id);
}