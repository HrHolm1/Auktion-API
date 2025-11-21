using Auktion_API.Models;
using Auktion_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auktion_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidController : ControllerBase
{
    private readonly BidService _bidService;

    public BidController(BidService bidService)
    {
        _bidService = bidService;
    }

    [HttpGet("lot/{lotId}")]
    public async Task<IActionResult> GetBidsBylot(int lotId)
    {
        var bids = await _bidService.GetBidsByLotIdAsync(lotId);
        return Ok(bids);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBidsByUser(string userId)
    {
        var bids = await _bidService.GetBidsByUserIdAsync(userId);
        return Ok(bids);
    }
    

    [HttpGet("lot/{lotId}/highest")]
    public async Task<IActionResult> GetHighestBidForLot(int lotId)
    {
        var bid = await _bidService.GetHighestBidForLotAsync(lotId);
        if (bid == null)
            return NotFound();

        return Ok(bid);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceBid(Bid bid)
    {
        var newBid = await _bidService.PlaceBidAsync(bid);
        return CreatedAtAction(nameof(GetBidsBylot), new { lotId = newBid.LotId }, newBid);
    }

}