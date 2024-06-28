using Application.Abstractions.Dapper;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetVarianceOfDurationTime;

public class GetVarianceOfDurationTimeQueryHandler : IRequestHandler<GetVarianceOfDurationTimeQuery, Result<float>>
{
    private readonly IMetricsRepository _repository;

    public GetVarianceOfDurationTimeQueryHandler(IMetricsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<float>> Handle(GetVarianceOfDurationTimeQuery request, CancellationToken cancellationToken)
    {
        var variance = await _repository.GetVarianceOfDurationTimeAsync();
        return Result<float>.Success(variance);
    }
}