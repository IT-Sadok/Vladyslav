using Application.Abstractions.Dapper;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetUsersCountByRole;

public class GetUsersCountByRoleQueryHandler : IRequestHandler<GetUsersCountByRoleQuery, Result<long>>
{
    private readonly IMetricsRepository _repository;

    public GetUsersCountByRoleQueryHandler(IMetricsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<long>> Handle(GetUsersCountByRoleQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.GetUsersCountByRoleAsync(request.RoleId);
        return Result<long>.Success(count);
    }
}