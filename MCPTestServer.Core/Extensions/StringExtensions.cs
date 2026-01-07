using System.Text.Json;

namespace MCPTestServer.Core.Extensions;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };

    extension(string json)
    {
        public T? FromJson<T>() => JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}
