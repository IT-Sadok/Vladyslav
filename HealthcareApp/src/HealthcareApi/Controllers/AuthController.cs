using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api")]
public class AuthController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AuthController(IUserAuthenticationService userAuthenticationService) =>
        this._userAuthenticationService = userAuthenticationService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
    {
        var token = await _userAuthenticationService.LoginAsync(loginUserDTO);
        if (token == null) return Unauthorized("Login failed");
        
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userAuthenticationService.RegisterAsync(registerUserDTO);
        if (result.Succeeded) return Ok();
        
        return BadRequest(result.Errors);
    }
}