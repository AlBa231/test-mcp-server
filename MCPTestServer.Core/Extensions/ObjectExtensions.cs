using System.Text.Json;

namespace MCPTestServer.Core.Extensions;

public static class ObjectExtensions
{
    extension(object obj)
    {
        public string ToJson() => JsonSerializer.Serialize(obj);
    }
}
