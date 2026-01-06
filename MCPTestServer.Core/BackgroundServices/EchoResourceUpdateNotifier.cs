using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace MCPTestServer.Core.BackgroundServices;

public class EchoResourceUpdateNotifier(IServiceProvider serviceProvider, ILogger<EchoResourceUpdateNotifier> logger) : BackgroundService
{
    private const int NotifyDelay = 10000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //var mcpServer = serviceProvider.GetRequiredService<IMcpServerPrimitive>();
        //while (!stoppingToken.IsCancellationRequested)
        //{
            await Task.Delay(NotifyDelay, stoppingToken);
        //    logger?.LogInformation("Send resource update notification");
        //    await mcpServer.SendNotificationAsync("hello://echo/best", stoppingToken);
        //}
    }
}
