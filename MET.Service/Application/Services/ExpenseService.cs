using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using MET.Service.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Application.Services;

public class ExpenseService(AppDbContext context) : IExpenseService
{
    public async Task<Expense> GetAsync(Guid id, CancellationToken ct = default)
    {
        var expense = await context.Set<Expense>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (expense is null)
            throw new KeyNotFoundException($"Expense {id} not found.");

        return expense;
    }
    
    public async Task<List<Expense>> ListAsync(CancellationToken ct = default)
    {
        return await context.Set<Expense>()
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Expense> CreateAsync(Expense expense, CancellationToken ct = default)
    {
        context.Set<Expense>().Add(expense);
        await context.SaveChangesAsync(ct);
        return expense;
    }

    public async Task<Expense> UpdateAsync(Expense expense, CancellationToken ct = default)
    {
        context.Entry(expense).State = EntityState.Modified;

        await context.SaveChangesAsync(ct);
        return expense;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var expense = await context.Set<Expense>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (expense is null)
            return;

        context.Set<Expense>().Remove(expense);
        await context.SaveChangesAsync(ct);
    }
}