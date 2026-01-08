using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using MET.Service.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Application.Services;

public class UserService(AppDbContext context) : IUserService
{
    public async Task<User> GetAsync(Guid id, CancellationToken ct = default)
    {
        var user = await context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        return user;
    }
    
    public async Task<List<User>> ListAsync(int skip, int take, CancellationToken ct = default)
    {
        return await context.Set<User>().Skip(skip).Take(take)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<User> CreateAsync(RegisterRequestDto request, CancellationToken ct = default)
    {
        // Check for existing user with same email
        var existingUser = await context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, ct);    

        if (existingUser != null)
        {
            // Handle user already exists case
            return null;
        }

        var result = context.Set<User>().Add(new User
        {
            Email = request.Email!,
            Name = request.Username!,
            Password = request.Password!,
            CreatedAt = DateTimeOffset.UtcNow
        });
        await context.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken ct = default)
    {
        context.Entry(user).State = EntityState.Modified;

        await context.SaveChangesAsync(ct);
        return user;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await context.Set<User>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (user is null)
            return;

        context.Set<User>().Remove(user);
        await context.SaveChangesAsync(ct);
    }
}