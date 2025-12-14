using ModelContextProtocol.Server;

namespace McpTestServer.Core.Prompts;

[McpServerPromptType]
public static class VacationPrompt
{
    [McpServerPrompt(Title = "Helps the user plan a trip based on preferences")]
    public static string TravelPlanner(
        string destination,
        int? duration,
        decimal? budget,
        string[]? interests
    )
    {
        string interestsText = interests is { Length: > 0 }
            ? string.Join(", ", interests)
            : "no specific interests";

        return $"""
                You are a professional travel planner.

                Plan a trip to {destination}.
                Duration: {(duration.HasValue ? $"{duration} days" : "flexible")}
                Budget: {(budget.HasValue ? budget.Value.ToString("C") : "not specified")}
                Interests: {interestsText}

                Provide a clear, structured itinerary.
                """;
    }
}
