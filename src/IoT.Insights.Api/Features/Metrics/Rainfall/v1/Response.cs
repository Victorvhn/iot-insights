using IoT.Insights.Api.HttpClients.DevicesApi.Responses;

namespace IoT.Insights.Api.Features.Metrics.Rainfall.v1;

/// <summary>
///     Represents the response of a get rainfall metrics request.
/// </summary>
/// <param name="Devices">A list devices with rainfall metrics.</param>
public record GetRainfallMetricsResponse(
    IEnumerable<DeviceInfoWithRainfallMetrics> Devices
);

/// <summary>
///     Represents a device with rainfall metrics.
/// </summary>
/// <param name="DeviceId">The id of the device.</param>
/// <param name="Description">The description of the device.</param>
/// <param name="Manufacturer">The manufacturer of the device.</param>
/// <param name="Url">The url of the device.</param>
/// <param name="AvailableCommands">The available commands for the device.</param>
/// <param name="RainfallMetrics">The rainfall metrics of the device.</param>
public record DeviceInfoWithRainfallMetrics(
    string DeviceId,
    string Description,
    string Manufacturer,
    string Url,
    List<CommandInfo> AvailableCommands,
    string? RainfallMetrics
) : DeviceInfo(DeviceId, Description, Manufacturer, Url, AvailableCommands);