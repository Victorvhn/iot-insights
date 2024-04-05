using System.Text.Json.Serialization;

namespace IoT.Insights.Api.HttpClients.DevicesApi.Responses;

public record DeviceInfo(
    [property: JsonPropertyName("Identifier")]
    string DeviceId,
    string Description,
    string Manufacturer,
    string Url,
    [property: JsonPropertyName("Commands")]
    List<CommandInfo> AvailableCommands
);