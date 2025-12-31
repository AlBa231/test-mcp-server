using McpTestServer.Core.Extensions;
using MCPTestServer.Lambda.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MCPTestServer.Lambda;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public string BasePath => Configuration["BasePath"] ?? "/labmda";

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddHttpClient()
            .AddMcpServer()
            .WithHttpTransport()
            .AddMcpTestServerFeatures();

        services.AddHealthChecks();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UsePathBase(BasePath);
        app.UseMcpExceptionHandling();

        app.UseHealthChecks("/health");

        app.UseEndpoints(builder => builder.MapMcp(BasePath));
    }
}