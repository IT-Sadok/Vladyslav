using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api")]
public class TestRoleController : Controller
{
    [HttpGet("test")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Test() => Ok("You are logged in!");

    [HttpGet("test/patient")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
    public IActionResult TestPatient() => Ok("You are logged as a patient!");

    [HttpGet("test/doctor")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]

    public IActionResult TestDoctor() => Ok("You are logged as a doctor!");
}
