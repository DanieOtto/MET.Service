using MET.Service.Application.DTOs;
using MET.Service.Application.DTOs.Utilities;
using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MET.Service.Controllers;

[ApiController]
[Route("api/v1/expenses")]
public class ExpenseController(IExpenseService service) : ControllerBase
{
        // GET /api/expenses/{id}
        [HttpGet($"{{id:guid}}", Name = "GetExpenseById")]
        public async Task<ActionResult<ExpenseDto>> Get([FromRoute]Guid id, CancellationToken ct = default)
        {
                var expense = await service.GetAsync(id, ct);
                if (expense is null) return NotFound();
                return Ok(expense);
        }

        // GET /api/expenses
        [HttpGet]
        public async Task<ActionResult<PagedList<Expense>>> List([FromQuery] ExpenseQuery query, CancellationToken ct = default)
        {
                var items = await service.ListAsync(query.Skip, query.Take, ct);
                var total = items.Count;
                return Ok(new PagedList<Expense>(items, query.PageNumber, query.PageSize, total));
        }
        
        // CREATE /api/expenses
        [HttpPost]
        public async Task<Expense> CreateAsync(Expense expense, CancellationToken ct = default)
        {
                var result = await service.CreateAsync(expense, ct);

                return result;
        }

        // UPDATE /api/expenses
        [HttpPatch]
        public async Task<Expense> UpdateAsync(Expense expense, CancellationToken ct = default)
        {
                var result = await service.UpdateAsync(expense, ct);
                return result;
        }

        // GET /api/expenses/summary/{id}
        [Route("summary")]
        [HttpGet("{id:guid}", Name = "GetExpenseSummaryById")]
        public async Task<ActionResult<ExpenseSummaryDto>> GetSummary([FromRoute] Guid id, CancellationToken ct = default)
        {
                var result = await service.CreateSummaryReport(id, ct);
                return Ok(result);
        }
        
        // DELETE /api/expenses/{id}
        [HttpDelete("{id:guid}")]
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
                await service.DeleteAsync(id, ct);
        }
}