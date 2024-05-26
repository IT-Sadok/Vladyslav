using Application.Abstractions;
using Application.DTOs.Login;
using Application.DTOs.Register;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api")]
public class AuthController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AuthController(IUserAuthenticationService userAuthenticationService) =>
        this._userAuthenticationService = userAuthenticationService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _userAuthenticationService.LoginAsync(loginUserDto);
        if (string.IsNullOrEmpty(result)) return Unauthorized("Login failed");
        
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _userAuthenticationService.RegisterAsync(registerUserDto);
        if (result.Succeeded) return Ok();
        
        return BadRequest(result.Errors);
    }
}