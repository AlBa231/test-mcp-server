using McpTestServer.Core.Extensions;
using MCPTestServer.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures();

if (builder.Configuration.UseAuthorization())
    builder.Services.AddMcpAuthorization(builder.Configuration);

var app = builder.Build();
app.UseRequestLogging();

if (builder.Configuration.UseAuthorization())
    app.UseMcpAuthorization().MapMcp("/test").RequireAuthorization();
else
    app.MapMcp("/test");

await app.RunAsync("http://localhost:3001");
