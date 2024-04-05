using System.Net.Http.Headers;
using AutoFixture;
using FluentAssertions;
using Flurl.Http.Testing;
using IoT.Insights.Api.Features.Metrics.Rainfall.v1;
using IoT.Insights.Api.HttpClients.DevicesApi.Responses;
using IoT.Insights.Api.TcpClients.Devices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Rony.Listeners;
using Rony.Net;

namespace IoT.Insights.Api.Tests.Features.Metrics.Rainfall.v1;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string AccessToken =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFkbWluIiwiZXhwIjozMzI2OTI0NDAwMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.47N-Vn2_h-uBA1r3R3BE8jjcILG25aL_9BN5kXS5gxc";

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Fixture _fixture = new();

    public EndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Test");

            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json"), false)
                .Build();
            builder.UseConfiguration(integrationConfig);
        });

        _factory.Server.PreserveExecutionContext = true;
    }

    [Fact]
    public async Task It_should_not_return_an_access_token_when_provided_credentials_are_invalid()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var deviceList = _fixture.CreateMany<Guid>(3).ToList();

        using var httpTest = new HttpTest();
        httpTest
            .ForCallsTo("https://my-test-api.com/device")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(deviceList);

        var expectedCommand = _fixture.Build<CommandInfo>()
            .With(w => w.Operation, "get_rainfall_intensity")
            .Create();

        var deviceInfo1 = _fixture.Build<DeviceInfo>()
            .With(w => w.DeviceId, deviceList[0].ToString())
            .With(w => w.Manufacturer, "PredictWeather")
            .Without(w => w.AvailableCommands)
            .Create();
        httpTest
            .ForCallsTo($"*{deviceInfo1.DeviceId}")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(deviceInfo1);

        var deviceInfo2 = _fixture.Build<DeviceInfo>()
            .With(w => w.DeviceId, deviceList[1].ToString())
            .With(w => w.Manufacturer, "WeatherData Inc.")
            .With(w => w.AvailableCommands, [expectedCommand])
            .Create();
        httpTest
            .ForCallsTo($"*{deviceInfo2.DeviceId}")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(deviceInfo2);

        var deviceInfo3 = _fixture.Build<DeviceInfo>()
            .With(w => w.DeviceId, deviceList[2].ToString())
            .With(w => w.Manufacturer, "PredictWeather")
            .With(w => w.Url, "127.0.0.1")
            .With(w => w.AvailableCommands, [_fixture.Create<CommandInfo>(), expectedCommand])
            .Create();
        httpTest
            .ForCallsTo($"*{deviceInfo3.DeviceId}")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(deviceInfo3);

        using var tcpServer = new MockServer(new TcpServer(CommandTcpClient.DefaultPort));
        tcpServer.Mock.Send("").Receive("0.5");
        tcpServer.Start();

        // Act
        var response = await client
            .GetAsync("api/v1/metrics/rainfall");

        // Assert
        response
            .Should()
            .BeSuccessful();
        response.Should().Satisfy<GetRainfallMetricsResponse>(model =>
            model
                .Devices
                .Should()
                .ContainSingle()
                .And
                .OnlyHaveUniqueItems(c => c.DeviceId)
                .And
                .SatisfyRespectively(s =>
                    {
                        s.DeviceId.Should().Be(deviceInfo3.DeviceId);
                        s.RainfallMetrics.Should().Be("0.5");
                    }
                )
        );
    }
}