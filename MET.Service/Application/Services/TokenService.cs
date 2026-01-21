using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MET.Service.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using MET.Service.Domain.Entities;

namespace MET.Service.Application.Services;

public sealed class TokenService(IConfiguration config) : ITokenService
{
    public string Create(User request)
    {
        var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];
        var accessTokenMinutes = int.Parse(config["Jwt:AccessTokenMinutes"] ?? "15");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, request.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(accessTokenMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
