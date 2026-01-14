using MET.Service.Application.DTOs;
using MET.Service.Domain.Entities;

namespace MET.Service.Application.Interfaces;

public interface IUserService
{
    Task<User> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<User>> ListAsync(Guid? id = null, string? email = null, int skip = 0, int take = 100, CancellationToken ct = default);
    Task<User> AuthenticateAsync(string email, string password);
    Task<User> RegisterUserAsync(RegisterRequestDto request, CancellationToken ct = default);
    Task<User> UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}