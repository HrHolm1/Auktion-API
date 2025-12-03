using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auktion_API.DataAccess;
using Auktion_API.DTOs;
using Auktion_API.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Auktion_API.Services;

public class AuthService
{
    private readonly AuctionContext _db;

    
    public AuthService(AuctionContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<User?> ValidateUserAsync(LoginRequest loginRequest)
    {
        var userToValidate = new User()
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        };
        
        var returnUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == userToValidate.Email && x.Password == userToValidate.Password);
        return returnUser;
    }

    public async Task<string> ValidateToken(RefreshRequestDto refDto)
    {
        
        if (string.IsNullOrEmpty(refDto.token)) 
            return "";
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("5f1bd9c614b1dcf074313362f2ceb290037f83a39a4f4ff5183de85fea183ace");
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = true,
            ValidIssuer = "emptydomain.com",

            ValidateAudience = true,
            ValidAudience = "emptydomain.com",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // no extra 5 min grace period
        };

        try
        {
            tokenHandler.ValidateToken(refDto.token, validationParameters, out SecurityToken validatedToken);
            return GenerateJwtToken("");
        }
        catch (Exception)
        {
            // Token expired or another error
            return "";
        }
    }
    
    public string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            //new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5f1bd9c614b1dcf074313362f2ceb290037f83a39a4f4ff5183de85fea183ace"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: "emptydomain.com",
            audience: "emptydomain.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}