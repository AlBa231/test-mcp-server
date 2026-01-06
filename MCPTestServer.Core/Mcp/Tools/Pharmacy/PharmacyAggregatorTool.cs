using System.ComponentModel;
using MCPTestServer.Core.Extensions;
using MCPTestServer.Core.Extensions;
using MCPTestServer.Core.Mcp.Tools.Pharmacy.Models;
using MCPTestServer.Core.Services;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace MCPTestServer.Core.Mcp.Tools.Pharmacy;

[McpServerToolType]
public static class PharmacyAggregatorTool
{
    [McpServerTool(Title = "Search for the best prices for drugs and their prices in multiple pharmacies"), Description("""
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
    public static async Task<IEnumerable<PharmacyDrugInfo>> FindDrugPricesAsync([Description("The drug name.")] string drugName, RequestContext<CallToolRequestParams> context)
    {
        context.EnsureUserInRole(McpUserRole.Tools);
        var logger = context.Server.AsClientLoggerProvider().CreateLogger("PharmacyLogger");

        PharmacyExtraInfo? extraInfo = await TryElicitPharmacyExtraInfoAsync(context.Server, logger);

        await CheckApteka911Async(extraInfo, logger);
        await CheckAncAsync(extraInfo, logger);
        await CheckPodorognikAsync(extraInfo, logger);

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
                },
                Required = ["location"]
            }
        });
        logger.Log(LogLevel.Information, "Elicit result - {Result}", elicitResult.ToJson());

        return elicitResult.Content?.ToJson().FromJson<PharmacyExtraInfo>();
    }

    private static async Task CheckPodorognikAsync(PharmacyExtraInfo? extraInfo, ILogger logger)
    {
        if (extraInfo?.CheckPodorognik ?? true)
        {
            logger.Log(LogLevel.Information, "Searching drugs in Podorognik...");
            await Task.Delay(2000);
        }
    }

    private static async Task CheckAncAsync(PharmacyExtraInfo? extraInfo, ILogger logger)
    {
        if (extraInfo?.CheckANC ?? true)
        {
            logger.Log(LogLevel.Information, "Searching drugs in ANC...");
            await Task.Delay(2000);
        }
    }

    private static async Task CheckApteka911Async(PharmacyExtraInfo? extraInfo, ILogger logger)
    {
        if (extraInfo?.CheckApteka911 ?? true)
        {
            logger.Log(LogLevel.Information, "Searching drugs in Apteka 911...");
            await Task.Delay(2000);
        }
    }
}