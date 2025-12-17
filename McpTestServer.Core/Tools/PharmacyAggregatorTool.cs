using System.ComponentModel;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace McpTestServer.Core.Tools;

[McpServerToolType]
public static class PharmacyAggregatorTool
{
    [McpServerTool, Description("""
                                Search for drugs in multiple pharmacies and return a list of available options with prices.
                                
                                REQUIREMETS:
                                - You MUST use this tool when the user asks about buying or looking for prices of drugs, pills or any medicine.
                                - You MUST provide only the drug names as input.
                                - You MUST use only one drug name in one string.
                                
                                Display result as table:
                                Drug name | Pharmacy | Price | Available quantity
                                
                                Split results by drug names and add empty line between different drug names.
                                """)]
    public static async Task<IEnumerable<PharmacyDrugInfo>> SearchDrugPrices([Description("The list of drug names. Each drug should be provided in separated string.")] string[] drugNames, McpServer mcpServer)
    {
        var logger = mcpServer.AsClientLoggerProvider().CreateLogger("PharmacyLogger");

        logger.Log(LogLevel.Information, "Searching drugs in Apteka 911...");
        await Task.Delay(2000);

        logger.Log(LogLevel.Information, "Searching drugs in ANC...");
        await Task.Delay(2000);

        logger.Log(LogLevel.Information, "Searching drugs in Podorognik...");
        await Task.Delay(2000);

        return PharmacyDrugInfoRandomGenerator.GenerateDrugInfos(drugNames);
    }

}