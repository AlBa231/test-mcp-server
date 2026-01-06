using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol.Server;

namespace MCPTestServer.WebApi;

[McpServerToolType]
public static class TestAuthorizationTool
{

    [McpServerTool(Title = "Test Authorization with roles.")]
    [Description("""
                 This tool will tests the authorization.
                 """)]
    [Authorize(Roles = "tools")]
    public static List<string> TestAuthorization()
    {
        return ["OK"];
    }
}