using Auktion_API.Models;
using Auktion_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auktion_API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound(); 
        
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var newUser = await _userService.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.Id)
            return BadRequest();

        var success = await _userService.UpdateAsync(user);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        
        
        return NoContent();
    }

    // Add authorize here?
    [HttpGet("{id}/won-lots")]
    public async Task<IActionResult> GetWonLots(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        var wonLots = await _userService.GetWonLotsAsync(id);
        return Ok(wonLots);
    }

}