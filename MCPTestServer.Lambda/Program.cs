using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MCPTestServer.Core.Extensions;
using MCPTestServer.Lambda.Extensions;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAWSLambdaHosting(LambdaEventSource.ApplicationLoadBalancer)
    .AddHttpClient()
    .AddMcpServer()
    .WithHttpTransport(opts => opts.Stateless = true)
    .AddMcpTestServerFeatures();

builder.Services.AddHealthChecks();

var appBase = builder.Configuration["BasePath"] ?? "/lambda";
var app = builder.Build();
app.UsePathBase(appBase);
app.UseMcpExceptionHandling();

app.Logger.Log(LogLevel.Information, "Mapping health to /health");
app.MapHealthChecks("/health");

app.MapMcp(appBase);

await app.RunAsync();