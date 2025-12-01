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
public class UsersController(IUserService service) : ControllerBase
{ 
        // GET /api/users/{id}
        [HttpGet($"{{id:guid}}", Name = "GetUserById")]
        public async Task<ActionResult<UserDto>> Get([FromRoute]Guid id, CancellationToken ct = default)
        {
                var user = await service.GetAsync(id, ct);
                if (user is null) return NotFound();
                return Ok(user);
        }

        // GET /api/users
        [HttpGet]
        public async Task<ActionResult<PagedList<User>>> List([FromQuery] UserQuery query, CancellationToken ct = default)
        {
                var items = await service.ListAsync(query.Skip, query.Take, ct);
                var total = items.Count;
                return Ok(new PagedList<User>(items, query.PageNumber, query.PageSize, total));
        }
        
        // CREATE /api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateAsync([FromBody] User user, CancellationToken ct = default)
        {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var created = await service.CreateAsync(user, ct);
                return CreatedAtRoute("GetUserById", new { id = created.Id }, created);
        }

        // UPDATE /api/users
        [HttpPatch]
        public async Task<User> UpdateAsync(User user, CancellationToken ct = default)
        {
                var result = await service.UpdateAsync(user, ct);
                return result;
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken ct = default)
        {
                await service.DeleteAsync(id, ct);
                return NoContent();
        }
}