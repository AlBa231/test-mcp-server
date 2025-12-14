using ModelContextProtocol.Server;

namespace McpTestServer.Core.Resources;

[McpServerResourceType]
public static class HelloResource
{
    [McpServerResource(MimeType = "text/html", UriTemplate = "hello://echo/{name}", Title = "Get hello message for specified name.")]
    public static string EchoHelloMessageToUser(string name)
    {
        return $"<h1>Hello!</h1><p> Nice to meet you, <b>{name}</b></p>!";
    }

    [McpServerResource(MimeType = "text/html", UriTemplate = "hello://echo/best", Title = "Get the best greetings message.")]
    public static string BestGreetings()
    {
        return "Hello! This is <b>the best</b> greetings to you!";
    }
}