using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MET.Service.Controllers;

[ApiController]
[AllowAnonymous]
[EnableRateLimiting("fixed")]
[Route("api/[controller]")]
public class AuthController(ITokenService _tokenService, IUserService _userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!String.IsNullOrEmpty(request.Username) && !String.IsNullOrEmpty(request.Password))
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);

            if (user != null)
            {
                var result = _tokenService.Create(user);
                return Ok(new { result });
            }
        }
        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (String.IsNullOrEmpty(request.Username) || String.IsNullOrEmpty(request.Password))
        {
            return BadRequest();
        }

        if (request.Password.Length < 8)
        {
            return BadRequest("Password must be at least 8 characters long.");
        }

        var user = await _userService.RegisterUserAsync(request);

        if (user != null)
        {
            var token = _tokenService.Create(user);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}