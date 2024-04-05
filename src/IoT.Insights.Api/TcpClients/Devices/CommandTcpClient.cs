using System.Net.Sockets;

namespace IoT.Insights.Api.TcpClients.Devices;

public class CommandTcpClient : ICommandTcpClient
{
    public const int DefaultPort = 8005;

    public async Task<string?> ExecuteCommand(string url, string command, CancellationToken cancellationToken = default)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(url, DefaultPort, cancellationToken);

        await using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        await using var writer = new StreamWriter(stream);

        await writer.WriteLineAsync(command);
        await writer.FlushAsync(cancellationToken);

        return await reader.ReadLineAsync(cancellationToken);
    }
}