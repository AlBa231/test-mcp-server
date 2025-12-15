using McpTestServer.Core.Extensions;
using MCPTestServer.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures();
    //.AddAuthorizationFilters();

var app = builder.Build();

app.UseRequestLogging();
app.MapMcp(); //.RequireAuthorization();


await app.RunAsync("http://localhost:3001");