using MCPTestServer.Core.Extensions;
using MCPTestServer.Core.Mcp.Tools.Pharmacy.Models;
using MCPTestServer.Core.Mcp.Tools.Pharmacy.Services;
using MCPTestServer.Core.Services;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPTestServer.Core.Mcp.Tools.Pharmacy;

[McpServerToolType]
public static class PharmacyAggregatorTool
{
    [McpServerTool(Title = "Search for the best prices for drugs and their prices in multiple pharmacies")]
    [Description("""
                 Search for drugs in multiple pharmacies and return a list of available options with prices.

                 Use this tool when the user asks about prices, cost, or buying medicine in pharmacies.
                 This tool returns real pricing data and should be preferred over generic explanations.

                 Example:
                 User: "Find me the best price for Aspirin"
                 Call tool with:
                 drugName = "Aspirin"

                 Display result as table, if possible:
                 Pharmacy | Price | Available quantity

                 """)]
    public static async Task<IEnumerable<PharmacyDrugInfo>> FindDrugPricesAsync([Description("The drug name.")] string drugName, RequestContext<CallToolRequestParams> context,
        IDrugLookupService drugLookupService)
    {
        context.EnsureUserInRole(McpUserRole.Tools);
        var clientLogger = context.Server.AsClientLoggerProvider().CreateLogger("PharmacyLogger");

        PharmacyExtraInfo? extraInfo = await TryElicitPharmacyExtraInfoAsync(context.Server, clientLogger);

        await drugLookupService.CheckApteka911Async(extraInfo, clientLogger);
        await drugLookupService.CheckAncAsync(extraInfo, clientLogger);
        await drugLookupService.CheckPodorognikAsync(extraInfo, clientLogger);

        return PharmacyDrugInfoRandomGenerator.GenerateDrugInfos([drugName]);
    }

    private static async Task<PharmacyExtraInfo?> TryElicitPharmacyExtraInfoAsync(McpServer mcpServer, ILogger logger)
    {
        try
        {
            return await ElicitPharmacyExtraInfoAsync(mcpServer, logger);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private static async Task<PharmacyExtraInfo?> ElicitPharmacyExtraInfoAsync(McpServer mcpServer, ILogger logger)
    {
        var elicitationId = Guid.NewGuid().ToString();

        var elicitResult = await mcpServer.ElicitAsync(new ElicitRequestParams
        {
            Message = "Please provide which pharmacies should I have to check.",
            Mode = "form",
            ElicitationId = elicitationId,
            RequestedSchema = new ElicitRequestParams.RequestSchema
            {
                Properties =
                {
                    {"checkApteka911", new ElicitRequestParams.BooleanSchema {Description = "Enable checking in Apteka911 pharmacy", Title = "Check apteka911.ua?", Default = true}},
                    {"checkANC", new ElicitRequestParams.BooleanSchema {Description = "Enable checking in ANC pharmacy", Title = "Check anc.ua?", Default = true}},
                    {"checkPodorognik", new ElicitRequestParams.BooleanSchema {Description = "Enable checking in Podorognik pharmacy", Title = "Check podorozhnyk.ua?", Default = true}},
                    {"location", new ElicitRequestParams.StringSchema {Description = "Your location (city) to check.", Title = "Location"}},
                    //{"maxDestinationKm", new ElicitRequestParams.NumberSchema {Description = "Maximum distance in kilometers to search for pharmacies from your location.", Default = 10}}
                    // NumberSchema is not supported yet in elicitation
                },
                Required = ["location"]
            }
        });
        logger.Log(LogLevel.Information, "Elicit result - {Result}", elicitResult.ToJson());

        return elicitResult.Content?.ToJson().FromJson<PharmacyExtraInfo>();
    }
}