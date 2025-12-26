using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpTestServer.Core.Resources;

public class UkraineHolidaysResource(HttpClient client)
{
    private const string HolidaysUrl = "https://publicholidays.com.ua";

    private string? _cachedHtml;

    [McpServerResource(MimeType = "text/html", UriTemplate = "doc://holidays/Ukraine", Title = "Ukrainian holidays")]
    [Description("This Resource contains information about all Public and National holidays in Ukrain. ")]
    public async Task<string> GetUkrainianHolidaysHtml()
    {
        return _cachedHtml ??= await client.GetStringAsync(HolidaysUrl);
    }
}