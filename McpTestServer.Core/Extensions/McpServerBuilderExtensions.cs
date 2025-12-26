using McpTestServer.Core.BackgroundServices;
using McpTestServer.Core.Prompts;
using McpTestServer.Core.Resources;
using McpTestServer.Core.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace McpTestServer.Core.Extensions;

public static class McpServerBuilderExtensions
{
    public static IMcpServerBuilder AddMcpTestServerFeatures(this IMcpServerBuilder builder)
    {
        builder.Services
            .AddSingleton<HttpClient>()
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
