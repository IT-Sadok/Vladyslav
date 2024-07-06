using Application.Abstractions.Dapper;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetMedianOfDurationTime;

public class GetMedianOfDurationTimeQueryHandler : IRequestHandler<GetMedianOfDurationTimeQuery, Result<float>>
{
    private readonly IMetricsRepository _repository;

    public GetMedianOfDurationTimeQueryHandler(IMetricsRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<float>> Handle(GetMedianOfDurationTimeQuery request, CancellationToken cancellationToken)
    {
        var median = await _repository.GetMedianOfDurationTimeAsync();
        return Result<float>.Success(median);
    }
}