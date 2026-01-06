namespace MCPTestServer.Core;

public class McpConfiguration
{
    public static McpConfiguration Instance { get; } = new ();

    public bool IsAuthorizationEnabled { get; set; }
}