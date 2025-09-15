using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using MET.Service.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MET.Service.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseService
{
    private readonly AppDbContext _context;
    public ExpenseRepository(AppDbContext context) => _context = context;

    public Task<Expense> GetByIdAsync(Guid id) =>
        _context.Expenses.FirstOrDefaultAsync(u => u.Id == id);

    public Task<List<Expense>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Expense expense)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}