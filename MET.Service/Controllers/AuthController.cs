using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MET.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtService;
    
    public AuthController(IJwtTokenService jwtService)
    {
        _jwtService = jwtService;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        if (!String.IsNullOrEmpty(model.Username) && !String.IsNullOrEmpty(model.Password))
        {
            var token = _jwtService.GenerateToken(model.Username, "1");
            return Ok(new { token });
        }
        return Unauthorized();
    }
}