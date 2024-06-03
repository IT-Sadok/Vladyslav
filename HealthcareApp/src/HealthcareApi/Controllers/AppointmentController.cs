using Healthcare.Application.Appointments.Commands;
using Healthcare.Application.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api/[controller]")]
public class AppointmentController : Controller
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("request")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
    public async Task<IActionResult> RequestAppointment([FromBody] BookAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result) return Ok("Appointment requested successfully");

        return BadRequest("Failed to book appointment");
    }

    [HttpGet("requested")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
    public async Task<IActionResult> GetRequestedAppointments([FromQuery] GetRequestedAppointmentsQuery query)
    {
        var appointments = await _mediator.Send(query);
        var result = appointments
            .Select(x => new { x.PatientId, x.AppointmentDate, x.EndTime, x.StartTime, x.Status });

        return Ok(result);
    }
}