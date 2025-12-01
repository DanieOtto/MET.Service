using MET.Service.Application.DTOs;
using MET.Service.Application.DTOs.Paging;
using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MET.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<Expense>> CreateAsync([FromBody] Expense expense, CancellationToken ct = default)
    {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await service.CreateAsync(expense, ct);
            return CreatedAtRoute("GetExpenseById", new { id = created.Id }, created);
    }

    // UPDATE /api/expenses
    [HttpPatch]
    public async Task<Expense> UpdateAsync(Expense expense, CancellationToken ct = default)
    {
            var result = await service.UpdateAsync(expense, ct);
            return result;
    }

    // GET /api/expenses/summary/{id}
    [HttpGet("summary/{id:guid}", Name = "GetExpenseSummaryById")]
    public async Task<ActionResult<ExpenseSummaryDto>> GetSummary([FromRoute] Guid id, CancellationToken ct = default)
    {
            var result = await service.CreateSummaryReport(id, ct);
            return Ok(result);
    }
        
    // DELETE /api/expenses/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct = default)
    {
            await service.DeleteAsync(id, ct);
            return NoContent();
    }
}