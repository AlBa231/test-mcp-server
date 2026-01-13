using MCPTestServer.Core;
using MCPTestServer.Core.Extensions;
using MCPTestServer.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

McpConfiguration.Instance.IsAuthorizationEnabled = builder.Configuration.UseAuthorization;

var mcpBuilder = builder.Services
    .AddHttpClient()
    .AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures();

builder.Services.AddHealthChecks();

if (McpConfiguration.Instance.IsAuthorizationEnabled)
{
    mcpBuilder.WithToolsFromAssembly()
              .AddAuthorizationFilters();
    builder.Services.AddMcpAuthorization(builder.Configuration);
}

var app = builder.Build();
app.UsePathBase(builder.Configuration.AppBasePath)
    .UseRequestLogging()
    .UseMcpExceptionHandling();

app.MapHealthChecks("/health");

if (McpConfiguration.Instance.IsAuthorizationEnabled)
    app.UseMcpAuthorization().MapMcp(builder.Configuration.AppBasePath).RequireAuthorization();
else
    app.MapMcp(builder.Configuration.AppBasePath);

await app.RunAsync();
