using System.Net.Sockets;
using FluentResults;

namespace IoT.Insights.Api.TcpClients.Devices;

public class CommandTcpClient(ILogger<CommandTcpClient> logger) : ICommandTcpClient
{
    public const int DefaultPort = 8005;

    public async  Task<Result<string?>> ExecuteCommand(string url, string command, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(url, DefaultPort, cancellationToken);

            await using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            await using var writer = new StreamWriter(stream);

            await writer.WriteLineAsync(command);
            await writer.FlushAsync(cancellationToken);

            return Result.Ok(await reader.ReadLineAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error returned from {Url}: {ErrorMessage}", url, ex.Message);
            return Result.Fail(ex.Message);
        }
    }
}