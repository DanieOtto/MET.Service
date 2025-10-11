using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MET.Service.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MET.Service.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _key;

    public JwtTokenService(string key)
    {
        _key = key;
    }

    public string GenerateToken(string username, string userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("uid", userId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}