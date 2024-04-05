using FluentResults;
using Flurl.Http;
using Flurl.Http.Configuration;
using IoT.Insights.Api.HttpClients.DevicesApi.Responses;

namespace IoT.Insights.Api.HttpClients.DevicesApi;

internal class DevicesApiClient(ILogger<DevicesApiClient> logger, IFlurlClientCache clients) : IDevicesApiClient
{
    public const string ApiClientName = "DevicesApiClient";

    private readonly IFlurlClient _devicesApiClient = clients.Get(ApiClientName);

    public async Task<Result<string[]>> GetDeviceListAsync(CancellationToken cancellationToken)
    {
        var request = _devicesApiClient
            .Request("device");

        try
        {
            var result = await request
                .GetAsync(cancellationToken: cancellationToken)
                .ReceiveJson<string[]>();

            return Result.Ok(result);
        }
        catch (FlurlHttpException ex)
        {
            var errorMessage = await ex.GetResponseStringAsync();

            logger.LogError(ex, "Error returned from {Url}: {ErrorMessage}", ex.Call.Request.Url, errorMessage);
            return Result.Fail(errorMessage);
        }
    }

    public async Task<Result<DeviceInfo>> GetDeviceDataAsync(string deviceId, CancellationToken cancellationToken)
    {
        var request = _devicesApiClient
            .Request("device", deviceId);

        try
        {
            var result = await request
                .GetAsync(cancellationToken: cancellationToken)
                .ReceiveJson<DeviceInfo>();

            return Result.Ok(result);
        }
        catch (FlurlHttpException ex)
        {
            var errorMessage = await ex.GetResponseStringAsync();

            logger.LogError(ex, "Error returned from {Url}: {ErrorMessage}", ex.Call.Request.Url, errorMessage);
            return Result.Fail(errorMessage);
        }
    }
}