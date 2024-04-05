using IoT.Insights.Api.Infrastructure.Routing;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

internal class Endpoint : IEndpoint
{
    public string[] Tags { get; } = ["Metrics"];

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("metrics/rainfall",
                async (HttpContext context, ISender sender, CancellationToken cancellationToken) =>
                {
                    var request = new GetRainfallMetricsRequest();

                    var result = await sender.Send(request, cancellationToken);

                    return result switch
                    {
                        { IsFailed: true } => Results.Problem(statusCode: 400, instance: context.Request.Path,
                            detail: string.Join(", ", result.Errors.Select(s => s.Message))),
                        { IsSuccess: true } => Results.Ok(result.Value),
                        _ => null
                    };
                })
            .WithName("RainfallMetrics")
            .WithDescription("Returns rainfall metrics for each device in the system.")
            .WithTags(Tags)
            .Produces<GetRainfallMetricsResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi()
            .RequireAuthorization();
    }
}