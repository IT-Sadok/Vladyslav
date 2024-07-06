using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetDoctorAppointmentsCount;

public record GetDoctorAppointmentsCountQuery(string DoctorId, 
    DateTime? FromDate = null, 
    DateTime? ToDate = null) : IRequest<Result<long>>;