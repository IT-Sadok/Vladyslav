using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetUsersCountByRole;

public record GetUsersCountByRoleQuery(int RoleId) : IRequest<Result<long>>;