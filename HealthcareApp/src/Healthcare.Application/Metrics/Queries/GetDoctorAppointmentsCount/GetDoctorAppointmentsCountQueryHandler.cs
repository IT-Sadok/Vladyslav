using Application.Abstractions.Dapper;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetDoctorAppointmentsCount;

internal class GetDoctorAppointmentsCountQueryHandler : IRequestHandler<GetDoctorAppointmentsCountQuery, Result<long>>
{
    private readonly IMetricsRepository _repository;

    public GetDoctorAppointmentsCountQueryHandler(IMetricsRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<long>> Handle(GetDoctorAppointmentsCountQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetDoctorAppointmentsCount(request.DoctorId);
        return Result<long>.Success(result);
    }
}