using IoT.Insights.Api.Infrastructure.Routing;

namespace IoT.Insights.Api.Metrics.Rainfall;

internal class Endpoint : IEndpoint
{
    public string[] Tags { get; set; } = ["Metrics"];

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("metrics/rainfall", () => "Rainfall metrics")
            .WithTags(Tags);
    }
}