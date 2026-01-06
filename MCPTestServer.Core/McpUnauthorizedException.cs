namespace MCPTestServer.Core;

public class McpUnauthorizedException : Exception
{
    public McpUnauthorizedException()
    {
    }

    public McpUnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public McpUnauthorizedException(string message) : base(message)
    {
    }
}