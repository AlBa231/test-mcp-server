using McpTestServer.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging
    .AddConsole(consoleLogOptions => consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace)
    .AddFile("Logs/mcp-server-{Date}.txt");
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .AddMcpTestServerFeatures();

await builder.Build().RunAsync();
