using Healthcare.Application.Metrics.Queries.GetDoctorAppointmentsCount;
using Healthcare.Application.Metrics.Queries.GetMedianOfDurationTime;
using Healthcare.Application.Metrics.Queries.GetUsersCountByRole;
using Healthcare.Application.Metrics.Queries.GetVarianceOfDurationTime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[Route("api/[controller]")]
public class MetricsController : Controller
{
    private readonly IMediator _mediator;

    public MetricsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-user-count-by-role/{roleId:int}")]
    public async Task<IActionResult> GetUserCountByRole(int roleId)
    {
        var result = await _mediator.Send(new GetUsersCountByRoleQuery(roleId));
        return Ok(result);
    }

    [HttpGet("get-doctor-appointments-count/{doctorId}")]
    public async Task<IActionResult> GetDoctorAppointmentsCount(string doctorId)
    {
        var result = await _mediator.Send(new GetDoctorAppointmentsCountQuery(doctorId));
        return Ok(result);
    }

    [HttpGet("get-doctor-appointments-count-by-time-range/{doctorId}")]
    public async Task<IActionResult> GetDoctorAppointmentsCount(string doctorId, [FromQuery] DateTime fromDate,
        DateTime toDate)
    {
        var result = await _mediator.Send(new GetDoctorAppointmentsCountQuery(doctorId, fromDate, toDate));
        return Ok(result);
    }

    [HttpGet("get-median-of-duration-time")]
    public async Task<IActionResult> GetMedian()
    {
        var result = await _mediator.Send(new GetMedianOfDurationTimeQuery());
        return Ok(result);
    }

    [HttpGet("get-variance-of-duration-time")]
    public async Task<IActionResult> GetVariance()
    {
        var result = await _mediator.Send(new GetVarianceOfDurationTimeQuery());
        return Ok(result);
    }
}