using System.Text.Json;

namespace MCPTestServer.Core.Extensions;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };

    public static T? FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}
