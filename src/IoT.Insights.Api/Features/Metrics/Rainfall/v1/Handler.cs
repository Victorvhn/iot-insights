using FluentResults;
using IoT.Insights.Api.HttpClients.DevicesApi;
using IoT.Insights.Api.HttpClients.DevicesApi.Responses;
using IoT.Insights.Api.TcpClients.Devices;
using Mediator;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

public class Handler(
    IDevicesApiClient devicesApiClient,
    ICommandTcpClient commandTcpClient)
    : IRequestHandler<GetRainfallMetricsRequest, Result<GetRainfallMetricsResponse>>
{
    private const string GetRainfallIntensityCommand = "get_rainfall_intensity";
    private readonly string[] _validDeviceManufacturers = ["PredictWeather"];

    public async ValueTask<Result<GetRainfallMetricsResponse>> Handle(GetRainfallMetricsRequest request,
        CancellationToken cancellationToken)
    {
        var deviceList = await devicesApiClient.GetDeviceListAsync(cancellationToken);
        if (deviceList.IsFailed)
        {
            return Result.Fail<GetRainfallMetricsResponse>(deviceList.Errors);
        }

        var deviceInfoTasks = deviceList
            .Value
            .Select(deviceId => GetDeviceInfo(deviceId, cancellationToken))
            .ToList();

        await Task.WhenAll(deviceInfoTasks);

        if (deviceInfoTasks.Exists(a => a.Result.IsFailed))
        {
            var errors = deviceInfoTasks
                .SelectMany(s => s.Result.Errors)
                .ToList();

            return Result.Fail<GetRainfallMetricsResponse>(errors);
        }

        var deviceData = deviceInfoTasks
            .Where(w => w.Result.Value != default)
            .Select(s => s.Result.Value);

        return Result.Ok(
            new GetRainfallMetricsResponse(deviceData!)
        );
    }

    private async Task<Result<DeviceInfoWithRainfallMetrics?>> GetDeviceInfo(string deviceId,
        CancellationToken cancellationToken)
    {
        var result = await devicesApiClient.GetDeviceDataAsync(deviceId, cancellationToken);
        if (result.IsFailed)
        {
            return Result.Fail<DeviceInfoWithRainfallMetrics?>(result.Errors);
        }

        var data = result.Value;

        if (!IsManufacturerValid(data) || !IsOperationAvailable(data))
        {
            return Result.Ok();
        }

        var rainfallMetrics =
            await commandTcpClient.ExecuteCommand(data.Url, GetRainfallIntensityCommand, cancellationToken);

        return new DeviceInfoWithRainfallMetrics(
            data.DeviceId,
            data.Description,
            data.Manufacturer,
            data.Url,
            data.AvailableCommands,
            rainfallMetrics
        );
    }

    private bool IsManufacturerValid(DeviceInfo data) =>
        _validDeviceManufacturers.Contains(data.Manufacturer);

    private static bool IsOperationAvailable(DeviceInfo data) =>
        data.AvailableCommands.Any(a => a.Operation == GetRainfallIntensityCommand);
}