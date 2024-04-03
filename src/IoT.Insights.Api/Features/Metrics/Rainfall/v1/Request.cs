using FluentResults;
using Mediator;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

public record GetRainfallMetricsRequest : IRequest<Result<GetRainfallMetricsResponse>>;