using IoT.Insights.Api.TcpClients.Devices;

namespace IoT.Insights.Api.Infrastructure.TcpClients;

internal static class Extensions
{
    public static void AddTcpClients(this IServiceCollection services)
    {
        services.AddTransient<ICommandTcpClient, CommandTcpClient>();
    }
}