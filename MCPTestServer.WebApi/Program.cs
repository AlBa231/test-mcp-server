using McpTestServer.Core.Extensions;
using MCPTestServer.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures();

builder.Services.AddHealthChecks();

if (builder.Configuration.UseAuthorization())
    builder.Services.AddMcpAuthorization(builder.Configuration);

var app = builder.Build();
app.UsePathBase(builder.Configuration.GetAppBasePath());
app.UseRequestLogging();
app.UseMcpExceptionHandling();

app.Logger.Log(LogLevel.Information, "Mapping health to /health");
app.MapHealthChecks("/health");

if (builder.Configuration.UseAuthorization())
    app.UseMcpAuthorization().MapMcp(builder.Configuration.GetAppBasePath()).RequireAuthorization();
else
    app.MapMcp(builder.Configuration.GetAppBasePath());

await app.RunAsync();
