using Auktion_API.DTOs;
using Auktion_API.Models;

namespace Auktion_API.Interfaces;

public interface IBidService
{
    Task<List<BidDto>> GetBidsByLotIdAsync(int lotId);
    Task<List<Bid>> GetBidsByUserIdAsync(int userId);
    Task<Bid> PlaceBidAsync(Bid bid);
    Task<Bid?> GetHighestBidForLotAsync(int lotId);
}