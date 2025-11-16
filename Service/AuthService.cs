using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyWebApiApp.Data;
using MyWebApiApp.Dto;
using MyWebApiApp.Models;
using Microsoft.IdentityModel.Tokens;
using MyWebApiApp.Iservice;

namespace MyWebApiApp.Service;

public class AuthService(AppDbContext _context, IConfiguration configuration) : IAuthService
{
    public async Task<string?> LoginUser(UserDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            return null;
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.HashedPassword, request.Password) == PasswordVerificationResult.Failed)
        {
            return null;
        }
        string token = CreateToken(user);
        return token;

    }

    public async Task<User?> RegisterUser(UserDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return null;
        }
        var user = new User();
        user.Username = request.Username;
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
        user.HashedPassword = hashedPassword;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}