using System.Text.Json.Serialization;

namespace IoT.Insights.Api.HttpClients.DevicesApi.Responses;

public record Command(
    [property: JsonPropertyName("Command")]
    string CommandString,
    List<CommandParameter> Parameters
);