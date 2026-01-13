using MCPTestServer.Core.Mcp.Tools.Pharmacy.Models;
using Microsoft.Extensions.Logging;

namespace MCPTestServer.Core.Mcp.Tools.Pharmacy.Services;

public interface IDrugLookupService
{
    Task CheckPodorognikAsync(PharmacyExtraInfo? extraInfo, ILogger? clientLogger = null);
    Task CheckAncAsync(PharmacyExtraInfo? extraInfo, ILogger? clientLogger = null);
    Task CheckApteka911Async(PharmacyExtraInfo? extraInfo, ILogger? clientLogger = null);
}