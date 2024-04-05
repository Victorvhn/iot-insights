using FluentResults;
using IoT.Insights.Api.HttpClients.DevicesApi.Responses;

namespace IoT.Insights.Api.HttpClients.DevicesApi;

public interface IDevicesApiClient
{
    Task<Result<string[]>> GetDeviceListAsync(CancellationToken cancellationToken);

    Task<Result<DeviceInfo>> GetDeviceDataAsync(string deviceId, CancellationToken cancellationToken);
}