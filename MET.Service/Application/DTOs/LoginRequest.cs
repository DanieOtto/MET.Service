namespace MET.Service.Application.DTOs;

public class LoginRequest
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string[] Roles { get; set; }
    public required string[] Scopes { get; set; }
}