using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Http;
using IoT.Insights.Api.Features.Authentication.SignIn.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IoT.Insights.Api.Tests.Features.Authentication.v1;

public class EndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task It_should_not_return_an_access_token_when_provided_credentials_are_invalid()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = _fixture.Build<SignInRequest>()
            .With(w => w.Username, "Wrong username")
            .With(w => w.Password, "Wrong password")
            .Create();

        // Act
        var response = await client.PostAsJsonAsync("api/v1/authentication/sign-in", request);

        // Assert
        response
            .StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
        response
            .Should()
            .HaveContentMatching<ProblemDetails>(x => x.Detail == "Invalid username or password");
    }

    [Fact]
    public async Task It_should_return_an_access_token_when_credentials_are_valid()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = _fixture.Build<SignInRequest>()
            .With(w => w.Username, "admin")
            .With(w => w.Password, "admin")
            .Create();

        // Act
        var response = await client.PostAsJsonAsync("api/v1/authentication/sign-in", request);

        // Assert
        response
            .Should()
            .BeSuccessful();
        response
            .Should()
            .HaveContentMatching<SignInResponse>(x => x.Token != string.Empty);
    }
}