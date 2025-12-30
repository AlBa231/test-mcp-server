using Microsoft.Extensions.Configuration;

namespace MCPTestServer.Lambda.Extensions;

public static class ConfigurationManagerExtensions
{
    public static string GetAppBasePath(this ConfigurationManager configurationManager)
    {
        return configurationManager["BasePath"] ?? "/";
    }
}
