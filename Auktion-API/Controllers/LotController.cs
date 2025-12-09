using Auktion_API.Models;
using Auktion_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auktion_API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LotController : ControllerBase
{
    private readonly LotService _lotService;
    private readonly IWebHostEnvironment _env;


    public LotController(LotService lotService, IWebHostEnvironment env)
    {
        _lotService = lotService;
        _env = env;
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
    
    [HttpGet("{id}/winner")]
    public async Task<IActionResult> GetWinnerById(int id)
    {
        var winnerEmail = await _lotService.GetWinnerEmailAsync(id);
        
        if (winnerEmail == null)
            return NotFound();
        
        return Ok(new { email = winnerEmail });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Lot lot)
    {
        var newLot = await _lotService.CreateAsync(lot);
        return CreatedAtAction(nameof(GetById), new { id = newLot.Id, newLot }, newLot);
    }

    [Authorize]
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

    [Authorize]
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

    // Image upload
    [HttpPost("{lotId}/images")]
    public async Task<IActionResult> UploadImages(int lotId, [FromForm] List<IFormFile> files)
    {
        if (files.Count == 0)
            return BadRequest("No files uploaded");

        var images = await _lotService.AddImagesAsync(lotId, files, _env.WebRootPath);

        if (images == null)
            return NotFound("Lot not found");

        return Ok(images);
    }
}