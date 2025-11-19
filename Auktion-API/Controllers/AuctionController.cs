using Auktion_API.Models;
using Auktion_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auktion_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly AuctionService _auctionService;

    public AuctionController(AuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    [HttpGet]
    public async Task<IActionResult> Getall()
    {
        var auctions = await _auctionService.GetAllAsync();
        return Ok(auctions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var auction = await _auctionService.GetByIdAsync(id);
        if (auction == null)
            return NotFound();
        return Ok(auction);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Auction auction)
    {
        var newAuction = await _auctionService.CreateAsync(auction);
        return CreatedAtAction(nameof(GetById), new { id = newAuction.Id }, newAuction);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Auction auction)
    {
        if (id != auction.Id)
            return BadRequest();

        var succes = await _auctionService.UpdateAsync(auction);
        if (!succes)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _auctionService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

}