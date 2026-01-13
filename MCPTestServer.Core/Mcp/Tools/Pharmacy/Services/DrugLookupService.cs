using MCPTestServer.Core.Mcp.Tools.Pharmacy.Models;
using Microsoft.Extensions.Logging;

namespace MCPTestServer.Core.Mcp.Tools.Pharmacy.Services;

public class DrugLookupService(ILogger<DrugLookupService> serverLogger) : IDrugLookupService
{
    public async Task CheckPodorognikAsync(PharmacyExtraInfo? extraInfo, ILogger? clientLogger = null)
    {
        if (extraInfo?.CheckPodorognik ?? true)
        {
            serverLogger.Log(LogLevel.Information, "(server) Searching drugs in Podorognik...");
            clientLogger?.Log(LogLevel.Information, "(client) Searching drugs in Podorognik...");
            await Task.Delay(2000);
        }
    }

    public async Task CheckAncAsync(PharmacyExtraInfo? extraInfo, ILogger? clientLogger = null)
    {
        if (extraInfo?.CheckANC ?? true)
        {
            serverLogger.Log(LogLevel.Information, "(server) Searching drugs in ANC...");
            clientLogger?.Log(LogLevel.Information, "(client) Searching drugs in ANC...");
            await Task.Delay(2000);
        }
    }

    public async Task CheckApteka911Async(PharmacyExtraInfo? extraInfo, ILogger? clientLogger = null)
    {
        if (extraInfo?.CheckApteka911 ?? true)
        {
            serverLogger.Log(LogLevel.Information, "(server) Searching drugs in Apteka 911...");
            clientLogger?.Log(LogLevel.Information, "(client) Searching drugs in Apteka 911...");
            await Task.Delay(2000);
        }
    }
}
