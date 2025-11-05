using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MET.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    
    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        if (!String.IsNullOrEmpty(model.Username) && !String.IsNullOrEmpty(model.Password))
        {
            var result = _tokenService.Create(model);
            return Ok(new { result });
        }
        return Unauthorized();
    }
}