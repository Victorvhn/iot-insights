namespace IoT.Insights.Api.Infrastructure.Routing;

internal interface IEndpoint
{
    protected string[] Tags { get; }

    void MapEndpoint(IEndpointRouteBuilder app);
}