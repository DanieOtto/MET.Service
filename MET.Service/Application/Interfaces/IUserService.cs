using MET.Service.Application.DTOs;
using MET.Service.Domain.Entities;

namespace MET.Service.Application.Interfaces;

public interface IUserService
{
    Task<User> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<User>> ListAsync(int skip, int take, CancellationToken ct = default);
    Task<User> CreateAsync(RegisterRequestDto request, CancellationToken ct = default);
    Task<User> UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}