using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using MET.Service.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MET.Service.Controllers;

[ApiController]
[EnableRateLimiting("fixed")]
[Route("api/[controller]")]
public class AuthController(ITokenService _tokenService, IUserService _userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginRequest>> Login([FromBody] LoginRequest request)
    {
        if (!String.IsNullOrEmpty(request.Username) && !String.IsNullOrEmpty(request.Password))
        {
            //var user = await _userService.ListAsync();

            //if (user != null)
            //{
            //    var result = _tokenService.Create(request);
            //    return Ok(new { result });
            //}
        }
        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (String.IsNullOrEmpty(request.Username) || String.IsNullOrEmpty(request.Password))
        {
            return BadRequest();
        }

        var result = await _userService.CreateAsync(request);

        if (result != null)
        {
            var token = _tokenService.Create(new LoginRequest 
            { 
                Id = result.Id,
                Password = request.Password!,
                Roles = request.Roles,
                Scopes = request.Scopes,
                Username = request.Username!
            });
            return Ok(new { token });
        }

        return Unauthorized();
    }
}