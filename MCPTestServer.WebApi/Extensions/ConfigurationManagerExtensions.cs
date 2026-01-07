namespace MCPTestServer.WebApi.Extensions;

public static class ConfigurationManagerExtensions
{
    extension(IConfigurationManager configurationManager)
    {
        public bool UseAuthorization => bool.TryParse(configurationManager["EnableAuthorization"], out bool enableAuthorization) && enableAuthorization;

        public string AppBasePath => configurationManager["BasePath"] ?? "/";
    }
}
