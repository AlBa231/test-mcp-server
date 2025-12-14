using ModelContextProtocol.Server;

namespace McpTestServer.Core.Resources;

[McpServerResourceType]
public class WeatherResource(HttpClient client)
{
    private static readonly Dictionary<string, string> WeatherByCityUrls =
        new (StringComparer.OrdinalIgnoreCase)
        {
            { "Kiev", "https://www.yr.no/en/content/2-703448/meteogram.svg" },
            { "Kharkiv", "https://www.yr.no/en/content/2-706483/meteogram.svg" }
        };

    [McpServerResource(MimeType = "image/svg+xml", UriTemplate = "weather://forecast/{city}", Title = "Get weather forecast for any city.")]
    public async Task<string> GetTodayWeatherAsync(string city)
    {
        return await client.GetStringAsync(WeatherByCityUrls[city]);
    }
}