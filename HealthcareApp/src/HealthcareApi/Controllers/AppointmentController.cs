using Domain.Constants;
using Healthcare.Application.Appointments.Commands.Book;
using Healthcare.Application.Appointments.Commands.Complete;
using Healthcare.Application.Appointments.Commands.Reject;
using Healthcare.Application.Appointments.Queries;
using Healthcare.Application.DTOs.Result;
using Healthcare.Appointments.Commands.Submit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Domain.Constants.UserRolesConstants;

namespace HealthcareApi.Controllers;

[Route("api/[controller]s")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AppointmentController : Controller
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("request")]
    [Authorize(Roles = Patient)]
    public async Task<IActionResult> RequestAppointment([FromBody] BookAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Ok("Appointment requested successfully");

        return BadRequest(result.Error);
    }

    [HttpGet]
    [Authorize(Roles = Doctor)]
    public async Task<IActionResult> GetAppointments([FromQuery] GetDoctorAppointmentsQuery query)
    {
        var appointments = await _mediator.Send(query);
        return Ok(appointments);
    }

    [HttpPatch("submit")]
    [Authorize(Roles = Doctor)]
    public async Task<IActionResult> SubmitAppointment([FromBody] SubmitAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult("Appointment has been successfully submitted");
    }

    [HttpPatch("reject")]
    [Authorize(Roles = Doctor)]
    public async Task<IActionResult> RejectAppointment([FromBody] RejectAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult("Appointment has been successfully rejected");
    }

    [HttpPatch("complete")]
    [Authorize(Roles = Doctor)]
    public async Task<IActionResult> CompleteAppointment( [FromBody] CompleteAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult("Appointment has been successfully completed");
    }
}