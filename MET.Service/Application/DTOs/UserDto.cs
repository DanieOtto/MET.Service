namespace MET.Service.Application.DTOs;

public class UserDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}