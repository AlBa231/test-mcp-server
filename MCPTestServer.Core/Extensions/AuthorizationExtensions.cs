using MCPTestServer.Core.Services;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.Security.Claims;

namespace MCPTestServer.Core.Extensions;

public static class AuthorizationExtensions
{
    public static void EnsureUserInRole(this RequestContext<CallToolRequestParams> context, McpUserRole role)
    {
        EnsureUserInRole(context.User, role);
    }
    public static void EnsureUserInRole(this RequestContext<ReadResourceRequestParams> context, McpUserRole role)
    {
        EnsureUserInRole(context.User, role);
    }

    private static void EnsureUserInRole(ClaimsPrincipal? user, McpUserRole role)
    {
        if (!McpConfiguration.Instance.IsAuthorizationEnabled) return;
        if (!user?.IsInRole(role.ToString().ToLowerInvariant()) ?? false)
            throw new McpUnauthorizedException($"Role '{role}' required");
    }
}