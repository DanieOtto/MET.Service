using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using MET.Service.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Application.Services;

public class UserService(AppDbContext context) : IUserService
{
    public async Task<User> GetAsync(Guid id, CancellationToken ct = default)
    {
        var expense = await context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (expense is null)
            throw new KeyNotFoundException($"User {id} not found.");

        return expense;
    }
    
    public async Task<List<User>> ListAsync(int skip, int take, CancellationToken ct = default)
    {
        return await context.Set<User>().Skip(skip).Take(take)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        context.Set<User>().Add(user);
        await context.SaveChangesAsync(ct);
        return user;
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