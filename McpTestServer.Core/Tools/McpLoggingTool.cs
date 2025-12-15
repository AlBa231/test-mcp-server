using System.ComponentModel;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace McpTestServer.Core.Tools;

[McpServerToolType]
public static class McpLoggingTool
{
    [McpServerTool(Title = "Test logging tool."), Description("Test logging with client and server logging.")]
    public static async Task<string> LogMessageAsync(McpServer mcpServer)
    {
        var logger = mcpServer.AsClientLoggerProvider().CreateLogger("McpLoggingTool");
        foreach (LogLevel logLevel in Enum.GetValues(typeof(LogLevel)))
        {
            logger.Log(logLevel, "Test log {LogLevel} type. Please wait...", logLevel.ToString());
            await Task.Delay(2000);
        }

        return "Test Log messages are complete";
    }
}
