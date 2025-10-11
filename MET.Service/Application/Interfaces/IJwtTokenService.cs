namespace MET.Service.Application.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(string username, string userId);
}