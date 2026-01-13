using MCPTestServer.Core.Mcp.Prompts;
using MCPTestServer.Core.Mcp.Resources;
using MCPTestServer.Core.Mcp.Tools.Currency;
using MCPTestServer.Core.Mcp.Tools.Pharmacy;
using MCPTestServer.Core.Mcp.Tools.Pharmacy.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MCPTestServer.Core.Extensions;

public static class McpServerBuilderExtensions
{
    public static IMcpServerBuilder AddMcpTestServerFeatures(this IMcpServerBuilder builder)
    {
        builder.Services
            .AddTransient<IDrugLookupService, DrugLookupService>();

        builder
            .WithToolsFromAssembly(typeof(PharmacyAggregatorTool).Assembly)
            .WithTools<CurrencyTool>()
            .WithResourcesFromAssembly(typeof(HelloResource).Assembly)
            .WithResources<WeatherResource>()
            .WithResources<UkraineHolidaysResource>()
            .WithPromptsFromAssembly(typeof(VacationPrompt).Assembly);

        return builder;
    }
}
