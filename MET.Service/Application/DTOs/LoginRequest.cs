namespace MET.Service.Application.DTOs;

public class LoginRequest
{
    public Guid? Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string[]? Roles { get; set; }
    public string[]? Scopes { get; set; }
}