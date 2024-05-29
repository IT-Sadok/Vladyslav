using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api/[controller]")]
public class TestRoleController : Controller
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Test() => Ok("You are logged in!");

    [HttpGet("patient")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
    public IActionResult TestPatient() => Ok("You are logged as a patient!");

    [HttpGet("doctor")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]

    public IActionResult TestDoctor() => Ok("You are logged as a doctor!");
}
