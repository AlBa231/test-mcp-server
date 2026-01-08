using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MCPTestServer.Core.Mcp.Resources;

[McpServerResourceType]
public class WeatherResource(HttpClient client)
{
    private static readonly Dictionary<string, string> WeatherByCityUrls =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Kiev", "https://www.yr.no/en/content/2-703448/meteogram.svg" },
            { "Kharkiv", "https://www.yr.no/en/content/2-706483/meteogram.svg" }
        };

    [McpServerResource(MimeType = "image/svg+xml", UriTemplate = "weather://forecast/{city}", Title = "Get weather forecast for any city.")]
    public async Task<string> GetTodayWeatherAsync(string city)
    {
        return await client.GetStringAsync(WeatherByCityUrls[city]);
    }


    [McpServerResource(MimeType = "image/svg+xml", UriTemplate = "weather://forecast/today/Kiev", Title = "Get weather forecast for today for Kiev.")]
    [Description("""
                 Use this resource to display current weather in Kiev
                 
                 Display the image directly in the chat.
                 """)]
    public async Task<string> GetTodayWeatherInKievAsync()
    {
        return await GetTodayWeatherAsync("Kiev");
    }
}