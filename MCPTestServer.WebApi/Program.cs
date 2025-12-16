using System.Security.Claims;
using McpTestServer.Core.Extensions;
using MCPTestServer.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModelContextProtocol.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var serverUrl = builder.Configuration["ServerUrl"] ?? throw new InvalidOperationException("ServerUrl must be defined in appSettings.");
var authorizationServerUrl = builder.Configuration["AuthorizationServerUrl"] ?? throw new InvalidOperationException("AuthorizationServerUrl must be defined in appSettings.");
var inspectorOrigins = builder.Configuration["InspectorOrigins"] ?? throw new InvalidOperationException("AuthorizationServerUrl must be defined in appSettings.");

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .AddMcpTestServerFeatures()
    .AddAuthorizationFilters();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = McpAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = authorizationServerUrl;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
    })
    .AddMcp(options =>
    {
        options.ResourceMetadata = new()
        {
            Resource = new Uri(serverUrl),
            AuthorizationServers = { new Uri(authorizationServerUrl) },
            ScopesSupported = ["mcp:tools"]
        };
    });

builder.Services.AddCors(o =>
{
    o.AddPolicy("mcp-inspector", p => p
        .WithOrigins(inspectorOrigins.Split(','))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("WWW-Authenticate"));
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("mcp-inspector");
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLogging();
app.MapMcp().RequireAuthorization();

await app.RunAsync("http://localhost:3001");
