using MET.Service.Domain.Entities;

namespace MET.Service.Application.Interfaces;

public interface IExpenseService
{
    Task<Expense> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Expense>> ListAsync(CancellationToken ct = default);
    Task<Expense> CreateAsync(Expense expense, CancellationToken ct = default);
    Task<Expense> UpdateAsync(Expense expense, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}