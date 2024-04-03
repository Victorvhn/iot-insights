using IoT.Insights.Api.Infrastructure.Routing;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Insights.Api.Features.Authentication.SignIn.v1;

internal class Endpoint : IEndpoint
{
    public string[] Tags { get; } = ["Authentication"];

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("authentication/sign-in", async (HttpContext context, SignInRequest request, ISender sender) =>
            {
                var command = new SignInCommand(request.Username, request.Password);

                var result = await sender.Send(command);

                return result switch
                {
                    { IsFailed: true } => Results.Problem(statusCode: 400, instance: context.Request.Path,
                        detail: string.Join(", ", result.Errors.Select(s => s.Message))),
                    { IsSuccess: true } => Results.Ok(result.Value),
                    _ => null
                };
            })
            .WithName("SignIn")
            .WithDescription("Returns an api access token for the provided credentials.")
            .WithTags(Tags)
            .Produces<SignInResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .AllowAnonymous();
    }
}