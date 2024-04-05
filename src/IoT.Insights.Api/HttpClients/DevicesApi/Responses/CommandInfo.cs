namespace IoT.Insights.Api.HttpClients.DevicesApi.Responses;

public record CommandInfo(
    string Operation,
    string Description,
    Command Command,
    string Result,
    string Format
);