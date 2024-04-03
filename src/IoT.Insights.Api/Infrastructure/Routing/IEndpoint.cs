namespace IoT.Insights.Api.Infrastructure.Routing;

internal interface IEndpoint
{
    protected string[] Tags { get; set; }

    void MapEndpoint(IEndpointRouteBuilder app);
}