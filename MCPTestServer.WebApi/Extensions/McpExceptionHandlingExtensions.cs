using MCPTestServer.Core.Extensions;

namespace MCPTestServer.WebApi.Extensions;

public static class McpExceptionHandlingExtensions
{
    public static IApplicationBuilder UseMcpExceptionHandling(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                ILogger logger = context.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("McpException");

                string traceId = context.TraceIdentifier;

                logger.LogError(
                    ex,
                    "Unhandled MCP exception. TraceId={TraceId}",
                    traceId
                );
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";

                var payload = new
                {
                    jsonrpc = "2.0",
                    id = TryExtractRpcId(context),
                    error = new
                    {
                        code = "INTERNAL_ERROR",
                        message = "An internal error occurred while executing the MCP request.",
                        data = new
                        {
                            traceId
                        }
                    }
                };

                await context.Response.WriteAsync(payload.ToJson());
            }
        });
    }

    private static object? TryExtractRpcId(HttpContext context) => context.Items.TryGetValue("jsonrpc.id", out var id) ? id : null;
}
