using McpTestServer.Core.BackgroundServices;
using MCPTestServer.Core.Mcp.Prompts;
using MCPTestServer.Core.Mcp.Resources;
using MCPTestServer.Core.Mcp.Tools.Currency;
using Microsoft.Extensions.DependencyInjection;

namespace McpTestServer.Core.Extensions;

public static class McpServerBuilderExtensions
{
    public static IMcpServerBuilder AddMcpTestServerFeatures(this IMcpServerBuilder builder)
    {
        builder.Services
            .AddHostedService<EchoResourceUpdateNotifier>();

        builder
            .WithToolsFromAssembly(typeof(CurrencyTool).Assembly)
            .WithTools<CurrencyTool>()
            .WithResourcesFromAssembly(typeof(WeatherResource).Assembly)
            .WithResources<WeatherResource>()
            .WithResources<UkraineHolidaysResource>()
            .WithPromptsFromAssembly(typeof(VacationPrompt).Assembly);

        return builder;
    }
}
