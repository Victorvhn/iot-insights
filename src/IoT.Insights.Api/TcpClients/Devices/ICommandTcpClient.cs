namespace IoT.Insights.Api.TcpClients.Devices;

public interface ICommandTcpClient
{
    Task<string?> ExecuteCommand(string url, string command, CancellationToken cancellationToken = default);
}