using Application.Abstractions.Dapper;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetDoctorsWithAppointments;

public class GetDoctorsWithAppointmentsQueryHandler : IRequestHandler<GetDoctorsWithAppointmentsQuery, Result<List<object>>>
{
    private readonly IMetricsRepository _repository;

    public GetDoctorsWithAppointmentsQueryHandler(IMetricsRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<List<object>>> Handle(GetDoctorsWithAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetDoctorsWithAppointments();
        return Result<List<object>>.Success(result);
    }
}