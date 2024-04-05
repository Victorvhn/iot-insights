using FluentResults;
using Mediator;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

/// <summary>
///     Represents a request to get rainfall metrics.
/// </summary>
public record GetRainfallMetricsRequest : IRequest<Result<GetRainfallMetricsResponse>>;