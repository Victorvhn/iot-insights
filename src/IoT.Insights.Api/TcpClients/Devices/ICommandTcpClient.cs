using FluentResults;

namespace IoT.Insights.Api.TcpClients.Devices;

public interface ICommandTcpClient
{
    Task<Result<string?>> ExecuteCommand(string url, string command, CancellationToken cancellationToken = default);
}