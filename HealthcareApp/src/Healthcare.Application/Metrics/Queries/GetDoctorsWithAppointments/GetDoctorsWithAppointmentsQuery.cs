using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetDoctorsWithAppointments;

public record GetDoctorsWithAppointmentsQuery() : IRequest<Result<List<object>>>;