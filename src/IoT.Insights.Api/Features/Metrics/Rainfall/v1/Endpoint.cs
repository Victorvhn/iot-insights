using IoT.Insights.Api.Infrastructure.Routing;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

internal class Endpoint : IEndpoint
{
    public string[] Tags { get; } = ["Metrics"];

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("metrics/rainfall", () => "Rainfall metrics")
            .WithTags(Tags)
            .RequireAuthorization();
    }
}