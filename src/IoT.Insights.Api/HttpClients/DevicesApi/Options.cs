namespace IoT.Insights.Api.HttpClients.DevicesApi;

public class DevicesApiOptions
{
    public required string BaseUrl { get; init; }
    public required int RetryCount { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}