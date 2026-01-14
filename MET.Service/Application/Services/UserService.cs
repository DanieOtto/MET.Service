using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using MET.Service.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Application.Services;

public class UserService(AppDbContext _context, IPasswordHasher<User> _passwordHasher) : IUserService
{
    public async Task<User> GetAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        return user;
    }

    public async Task<List<User>> ListAsync(Guid? id = null, string? email = null, int skip = 0, int take = 100, CancellationToken ct = default)
    {
        var query = _context.Set<User>().AsQueryable();

        if (id.HasValue)
        {
            query = query.Where(u => u.Id != id.Value);
        }

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(u => u.Email == email);
        }

        return await query.Skip(skip).Take(take)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<User> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with email {email} not found.");
        }

        // Verify the password
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash!,
            password
        );

        return result == PasswordVerificationResult.Success ? user : null;
    }

    public async Task<User> RegisterUserAsync(RegisterRequestDto request, CancellationToken ct = default)
    {
        var existingUser = await _context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email, ct);    

        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with the same email already exists.");
        }

        User user = new User
        {
            Email = request.Email!,
            Name = request.Username!,
            CreatedAt = DateTimeOffset.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password!);

        var result = _context.Set<User>().Add(user);

        await _context.SaveChangesAsync(ct);
        return result.Entity;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync(ct);
        return user;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (user is null)
            return;

        _context.Set<User>().Remove(user);
        await _context.SaveChangesAsync(ct);
    }
}