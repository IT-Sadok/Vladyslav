using Healthcare.Application.Appointments.Commands.Book;
using Healthcare.Application.Appointments.Commands.Complete;
using Healthcare.Application.Appointments.Commands.Reject;
using Healthcare.Application.Appointments.Queries;
using Healthcare.Appointments.Commands.Submit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api/[controller]s")]
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
            .Select(x => new { x.AppointmentId, x.PatientId, x.AppointmentDate, x.EndTime, x.StartTime, x.Status });

        return Ok(result);
    }

    [HttpPatch("submit")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
    public async Task<IActionResult> SubmitAppointment([FromBody] SubmitAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result) return Ok("Appointment has been successfully submitted");
        
        return BadRequest("Failed to submit appointment");
    }

    [HttpPatch("reject")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
    public async Task<IActionResult> RejectAppointment([FromBody] RejectAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result) return Ok("Appointment has been successfully rejected");
        
        return BadRequest("Failed to reject appointment");
    }

    [HttpPatch("complete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
    public async Task<IActionResult> CompleteAppointment( [FromBody] CompleteAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result) return Ok("Appointment has been successfully completed");
        
        return BadRequest("Failed to complete appointment");
    }
}