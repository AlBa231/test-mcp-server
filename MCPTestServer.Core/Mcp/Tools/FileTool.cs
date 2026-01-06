using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPTestServer.Core.Mcp.Tools;

[McpServerToolType]
public static class FileTool
{
    [McpServerTool(Title = "List the files in the roots folders.")]
    [Description("""
                 This tool will return a list of files.
                 
                 Call this tool when the user asks about listing the files.
                 
                 It uses roots to find boundaries. If no roots specified - returns Roots is empty message.
                 """)]
    public static async Task<List<string>> ListFilesByRoot(RequestContext<CallToolRequestParams> context)
    {
        var files = await TryGetServerRootsAsync(context.Server);

        return files.Count == 0 ? ["Roots is empty"] : files;
    }

    private static async Task<List<string>> TryGetServerRootsAsync(McpServer server)
    {
        try
        {
            var roots = await server.RequestRootsAsync(new ListRootsRequestParams());

            return roots.Roots.Select(r => r.Uri).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [e.Message];
        }
    }
}