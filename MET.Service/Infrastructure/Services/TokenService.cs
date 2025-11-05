using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace MET.Service.Infrastructure.Services;

public sealed class TokenService(string key) : ITokenService
{
    public string Create(User user)
    {
        
        return string.Empty;
    }
}
