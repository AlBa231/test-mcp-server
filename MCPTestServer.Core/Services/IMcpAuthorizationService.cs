namespace MCPTestServer.Core.Services;

public interface IMcpAuthorizationService
{
    public bool IsInRole(McpUserRole role);
    public void EnsureUserInRole(McpUserRole role);
}