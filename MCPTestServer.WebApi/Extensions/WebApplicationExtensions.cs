namespace MCPTestServer.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if (!context.Request.Path.Value?.Contains("/health", StringComparison.OrdinalIgnoreCase) ?? false) 
                await LogCurrentRequest(context);

            await next();
        });
    }

    private static async Task LogCurrentRequest(HttpContext context)
    {
        context.Request.EnableBuffering();

        Console.WriteLine("============ HTTP REQUEST ============");
        Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
        Console.WriteLine("Headers:");
        foreach (var header in context.Request.Headers)
            Console.WriteLine($"  {header.Key}: {header.Value}");

        if (context.Request is { ContentLength: > 0, Body.CanRead: true })
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            string bodyText = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            Console.WriteLine("Body:");
            Console.WriteLine(bodyText);
        }
        else
        {
            Console.WriteLine("Body: <empty>");
        }

        Console.WriteLine("======================================");
    }

    public static WebApplication UseMcpAuthorization(this WebApplication app)
    {
        app.UseCors("mcp-inspector");
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
