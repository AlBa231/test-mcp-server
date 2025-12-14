using McpTestServer.Core.Extensions;
using MCPTestServer.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures();

var app = builder.Build();

app.UseRequestLogging();
app.MapMcp();

await app.RunAsync("http://localhost:3001");