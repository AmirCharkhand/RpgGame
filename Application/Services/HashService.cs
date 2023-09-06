using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using RPG.Application.Services.Contracts;
using RPG.Domain.Models;

namespace RPG.Application.Services;

public class HashService : IHashService
{
    private readonly IConfiguration _configuration;

    public HashService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void CreateHashWithSalt(string text, out byte[] textHash, out byte[] salt)
    {
        var hmac = new HMACSHA512();
        salt = hmac.Key;
        textHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));
    }

    public bool VerifyHash(string text, byte[] hashedText, byte[] salt)
    {
        var hmac = new HMACSHA512(salt);
        var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));
        return computeHash.SequenceEqual(hashedText);
    }

    public string CreateJwt(User user)
    {
        var secretKey = _configuration["RpgSecret"] ?? throw new AuthenticationException("Secret Key not Found");

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}