using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MET.Service.Controllers;

[ApiController]
[Route("expenses")]
public class ExpenseController(IExpenseService expenseService) : Controller
{
        public async Task<Expense> Get(Guid id, CancellationToken ct = default)
        {
                var result = await expenseService.GetAsync(id, ct);

                return result;
        }

        public async Task<List<Expense>> List(CancellationToken ct = default)
        {
                var result = await expenseService.ListAsync(ct);

                return result;
        }

        public async Task<Expense> CreateAsync(Expense expense, CancellationToken ct = default)
        {
                var result = await expenseService.CreateAsync(expense, ct);

                return result;
        }

        public async Task UpdateAsync(Expense expense, CancellationToken ct = default)
        {
                var result = await expenseService.UpdateAsync(expense, ct);
        }
}