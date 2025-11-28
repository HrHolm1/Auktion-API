using Auktion_API.DataAccess;
using Auktion_API.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

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
            Username = loginRequest.Email,
            Password = loginRequest.Password
        };
        
        var returnUser = await _db.Users.FirstOrDefaultAsync(x => x.Username == userToValidate.Username);
        return returnUser;
    }
}