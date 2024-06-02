using Healthcare.Application.Appointments.Commands;
using MediatR;
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
    
    [HttpPost("book")]
    public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result) return Ok("Appointment booked successfully");
        return BadRequest("Failed to book appointment");
    }
}