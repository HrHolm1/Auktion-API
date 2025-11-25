using Auktion_API.Models;

namespace Auktion_API.Services;

public interface IBidService
{
    Task<List<Bid>> GetBidsByLotIdAsync(int lotId);
    Task<List<Bid>> GetBidsByUserIdAsync(int userId);
    Task<Bid> PlaceBidAsync(Bid bid);
    Task<Bid?> GetHighestBidForLotAsync(int lotId);
}