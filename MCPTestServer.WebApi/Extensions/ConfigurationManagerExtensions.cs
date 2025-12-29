namespace MCPTestServer.WebApi.Extensions;

public static class ConfigurationManagerExtensions
{
    public static bool UseAuthorization(this IConfigurationManager configurationManager)
    {
        return bool.TryParse(configurationManager["EnableAuthorization"], out bool enableAuthorization) && enableAuthorization;
    }

    public static string GetAppBasePath(this ConfigurationManager configurationManager)
    {
        return configurationManager["BasePath"] ?? "/";
    }
}
