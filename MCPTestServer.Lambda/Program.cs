using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using McpTestServer.Core.Extensions;
using MCPTestServer.Lambda.Extensions;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient()
    .AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures();

builder.Services.AddHealthChecks();

var app = builder.Build();
app.UsePathBase(builder.Configuration.GetAppBasePath());
app.UseMcpExceptionHandling();

app.Logger.Log(LogLevel.Information, "Mapping health to /health");
app.MapHealthChecks("/health");

app.MapMcp(builder.Configuration.GetAppBasePath());

await app.RunAsync();
