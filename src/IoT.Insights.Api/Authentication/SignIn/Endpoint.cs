using IoT.Insights.Api.Infrastructure.Routing;

namespace IoT.Insights.Api.Authentication.SignIn;

internal class Endpoint : IEndpoint
{
    public string[] Tags { get; set; } = ["Authentication"];

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authentication/sign-in", () => "Sign In")
            .WithTags(Tags);
    }
}