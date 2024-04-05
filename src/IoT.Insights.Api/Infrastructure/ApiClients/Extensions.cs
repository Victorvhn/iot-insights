using Flurl.Http;
using Flurl.Http.Configuration;
using IoT.Insights.Api.HttpClients.DevicesApi;
using Polly;
using Polly.Retry;

namespace IoT.Insights.Api.Infrastructure.ApiClients;

internal static class Extensions
{
    public static void AddApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DevicesApiOptions>(configuration.GetSection("Apis:Devices"));

        var devicesApiOptions = configuration.GetSection("Apis:Devices").Get<DevicesApiOptions>();
        ArgumentNullException.ThrowIfNull(devicesApiOptions);

        var policy = BuildRetryPolicy(services, devicesApiOptions);

        services.AddSingleton<IFlurlClientCache>(_ => new FlurlClientCache()
            .WithDefaults(builder => { builder.AddMiddleware(() => new PollyHandler(policy)); })
            .Add(DevicesApiClient.ApiClientName, devicesApiOptions.BaseUrl, options =>
            {
                options
                    .WithBasicAuth(devicesApiOptions.Username, devicesApiOptions.Password);
            })
        );

        services.AddSingleton<IDevicesApiClient, DevicesApiClient>();
    }

    private static AsyncRetryPolicy<HttpResponseMessage> BuildRetryPolicy(IServiceCollection services,
        DevicesApiOptions devicesApiOptions)
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .RetryAsync(devicesApiOptions.RetryCount, (exception, retryCount) =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<DevicesApiClient>>();

                logger.LogWarning(exception.Exception, "Retry {RetryCount} of {PolicyKey}", retryCount,
                    DevicesApiClient.ApiClientName);
            });
    }
}