using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModelContextProtocol.AspNetCore.Authentication;

namespace MCPTestServer.WebApi.Extensions;

public static class ServiceCollectionAuthorizationExtensions
{
    public static IServiceCollection AddMcpAuthorization(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        var serverUrl = configurationManager["ServerUrl"] ?? throw new InvalidOperationException("ServerUrl must be defined in appSettings.");
        var authorizationServerUrl = configurationManager["AuthorizationServerUrl"] ?? throw new InvalidOperationException("AuthorizationServerUrl must be defined in appSettings.");
        var inspectorOrigins = configurationManager["InspectorOrigins"] ?? throw new InvalidOperationException("AuthorizationServerUrl must be defined in appSettings.");

        services
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

        services.AddCors(o =>
        {
            o.AddPolicy("mcp-inspector", p => p
                .WithOrigins(inspectorOrigins.Split(','))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("WWW-Authenticate"));
        });

        services.AddAuthorization();

        return services;
    }
}
