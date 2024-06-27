using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetVarianceOfDurationTime;

public record GetVarianceOfDurationTimeQuery() : IRequest<Result<float>>;