using MET.Service.Domain.Entities;

namespace MET.Service.Application.Interfaces;

public interface IExpenseService
{
    Task<Expense> GetByIdAsync(Guid id);
    Task<List<Expense>> GetAllAsync();
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Guid id);
}