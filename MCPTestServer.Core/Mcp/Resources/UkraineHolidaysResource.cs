using System.ComponentModel;
using MCPTestServer.Core.Extensions;
using MCPTestServer.Core.Services;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace MCPTestServer.Core.Mcp.Resources;

public class UkraineHolidaysResource(HttpClient client)
{
    private const string HolidaysUrl = "https://www.google.com/search?q=ukraine+holidays";

    private string? _cachedHtml;

    [McpServerResource(MimeType = "text/html", UriTemplate = "doc://holidays/Ukraine", Title = "Ukrainian holidays")]
    [Description("This Resource contains information about all Public and National holidays in Ukrain. ")]
    public async Task<string> GetUkrainianHolidaysHtml(RequestContext<ReadResourceRequestParams> context)
    {
        context.EnsureUserInRole(McpUserRole.Resources);
        return _cachedHtml ??= await client.GetStringAsync(HolidaysUrl);
    }
}