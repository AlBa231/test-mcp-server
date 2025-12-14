using System.Text.Json;

namespace McpTestServer.Core.Extensions;

public static class ObjectExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
