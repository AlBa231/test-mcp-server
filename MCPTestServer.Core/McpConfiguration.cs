namespace MCPTestServer.Core;

/// <summary>
/// This Singleton class holds some configuration settings related to Authorization for the MCP (Model Context Protocol) server and intended only for Development use.
/// </summary>
public class McpConfiguration
{
    public static McpConfiguration Instance { get; } = new();

    public bool IsAuthorizationEnabled { get; set; }
}