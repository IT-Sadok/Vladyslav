using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Metrics.Queries.GetMedianOfDurationTime;

public record GetMedianOfDurationTimeQuery() : IRequest<Result<float>>;