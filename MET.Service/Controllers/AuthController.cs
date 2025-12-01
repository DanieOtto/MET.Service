using MET.Service.Application.DTOs;
using MET.Service.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MET.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ITokenService _tokenService, IUserService _userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!String.IsNullOrEmpty(request.Username) && !String.IsNullOrEmpty(request.Password))
        {
            var user = _userService.GetAsync(request.Id);

            if (user == null)
            {
                var result = _tokenService.Create(request);
                return Ok(new { result });    
            }
        }
        return Unauthorized();
    }
}