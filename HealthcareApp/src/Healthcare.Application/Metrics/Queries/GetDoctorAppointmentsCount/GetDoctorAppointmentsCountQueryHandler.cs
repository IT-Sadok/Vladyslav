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
        long result = 0;
        if (request is { FromDate: not null, ToDate: not null })
        {
            result = await _repository.GetDoctorsAppointmentsCountAsync(request.DoctorId, request.FromDate, request.ToDate);
            return Result<long>.Success(result);
        }
        
        result = await _repository.GetDoctorsAppointmentsCountAsync(request.DoctorId);
        return Result<long>.Success(result);
    }
}