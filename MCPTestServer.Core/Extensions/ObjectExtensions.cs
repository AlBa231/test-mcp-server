using System.Text.Json;

namespace MCPTestServer.Core.Extensions;

public static class ObjectExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
