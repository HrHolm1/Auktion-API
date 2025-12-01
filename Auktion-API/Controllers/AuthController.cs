using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auktion_API.DTOs;
using Auktion_API.Models;
using Auktion_API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Auktion_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly AuthService _authService;
    
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshRequestDto refDto)
    {
        var newToken = await _authService.ValidateToken(refDto);

        if (newToken == "")
            return BadRequest("Invalid token");

        var r = new RefreshRequestDto
        {
            token = newToken,
        };
        
        return Ok(r);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        // Validate email and password
        var user = await _authService.ValidateUserAsync(loginRequest);

        if (user == null) return Unauthorized();
        
        var token = _authService.GenerateJwtToken(loginRequest.Email);
        var loginResponse = new LoginResponse()
        {
            Token = token,
            Role = user.Role,
            userId = user.Id,
            username = user.Username
        };
        return Ok(loginResponse);
    }
}