using Healthcare.Application.Schedules.Querries.AvailableTimeSlots;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api/[controller]")]
public class ScheduleController : Controller
{
    private readonly IMediator _mediator;

    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAvailableTimeSlots([FromQuery] GetAvailableSlotsQuery query)
    {
        var slots = await _mediator.Send(query);
        return Ok(slots);
    }
}