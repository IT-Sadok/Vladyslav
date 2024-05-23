using Healthcare.Application.Contracts;
using Healthcare.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api")]
public class AuthController : Controller
{
    private readonly IUser user;
    public AuthController(IUser user) => this.user = user;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login( [FromBody] LoginUserDTO loginUserDTO)
    {
        var result = await user.LoginUserAsync(loginUserDTO);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register( [FromBody] RegisterUserDTO registerUserDTO)
    {
        var result = await user.RegisterUserAsync(registerUserDTO);
        return Ok(result);
    }
}
