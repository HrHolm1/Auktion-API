using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _authService.ValidateUserAsync(loginRequest);

        if (user == null) return Unauthorized();
        
        var token = GenerateJwtToken(loginRequest.Email);
        var loginResponse = new LoginResponse()
        {
            Token = token,
            Role = user.Role,
            userId = user.Id,
            username = user.Username
        };
        return Ok(loginResponse);
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5f1bd9c614b1dcf074313362f2ceb290037f83a39a4f4ff5183de85fea183ace"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: "emptydomain.com",
            audience: "emptydomain.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}