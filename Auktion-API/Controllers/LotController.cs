using Auktion_API.Models;
using Auktion_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auktion_API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LotController : ControllerBase
{
    private readonly LotService _lotService;

    public LotController(LotService lotService)
    {
        _lotService = lotService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lots = await _lotService.GetAllAsync();
        return Ok(lots);
    }
    
    [HttpGet("auction/{auctionId:int}")]
    public async Task<IActionResult> GetAllFromId(int auctionId)
    {
        var lots = await _lotService.GetAllFromAuctionAsync(auctionId);
        return Ok(lots);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lot = await _lotService.GetByIdAsync(id);
        if (lot == null)
            return NotFound();

        return Ok(lot);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Lot lot)
    {
        var newLot = await _lotService.CreateAsync(lot);
        return CreatedAtAction(nameof(GetById), new { id = newLot.Id, newLot }, newLot);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Lot lot)
    {
        if (id != lot.Id)
            return BadRequest();

        var success = await _lotService.UpdateAsync(lot);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _lotService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{auctionId}/{lotNumber}")]
    public async Task<IActionResult> GetByAuctionId(int auctionId, int lotNumber)
    {
        var lot = await _lotService.GetLotByAuctionIdAsync(auctionId, lotNumber);
        if (lot == null)
            return NotFound();

        return Ok(lot);
    }
}