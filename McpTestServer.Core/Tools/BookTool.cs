using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.ComponentModel;
using McpTestServer.Core.Extensions;
using McpTestServer.Core.Models;
using Microsoft.Extensions.AI;

namespace McpTestServer.Core.Tools;

[McpServerToolType]
public static class BookTool
{
    private static readonly List<string> RecommendedBooks =
    [
        "1. Jon Skeet — C# in Depth (4th Edition)",
        "2. Andrew Troelsen, Philip Japikse — Pro C# 11 and the .NET 7 Platform",
        "3. Joseph Albahari — C# 12 in a Nutshell",
        "4. Jeffrey Richter — CLR via C#",
        "5. Adam Freeman — Pro ASP.NET Core 8",
        "6. Mark J. Price — C# 12 and .NET 8 – Modern Cross-Platform Development",
        "7. Steve Smith — Architecting Modern Web Applications with ASP.NET Core",
        "8. Robert C. Martin — Clean Code",
        "9. Robert C. Martin — Clean Architecture",
        "10. Martin Fowler — Refactoring (2nd Edition)",
        "11. Martin Kleppmann — Designing Data-Intensive Applications",
        "12. Eric Evans — Domain-Driven Design: Tackling Complexity in the Heart of Software",
        "13. Vaughn Vernon — Implementing Domain-Driven Design",
        "14. Roy Osherove — The Art of Unit Testing (2nd Edition)",
        "15. Gerard Meszaros — xUnit Test Patterns",
        "16. Stephen Cleary — Concurrency in C# Cookbook",
        "17. Sasha Goldshtein — Pro .NET Performance",
        "18. Andrew Hunt, David Thomas — The Pragmatic Programmer",
        "19. Sebastian Raschka — Machine Learning with PyTorch and Scikit-Learn (для AI фундаменту)",
        "20. Nishant Sivakumar — Practical Artificial Intelligence for .NET Developers"
    ];

    private static bool _initialized;

    [McpServerTool(Title = "List the recommended test book titles.")]
    [Description("""
                 This tool will return a list of recommended book titles for software developers.
                 
                 Use it when the user asks for book recommendations related to programming, software development, or C#.
                 The tool will ask for user consent before loading the book list for the first time.
                 
                 The tool may use sampling to filter the book list based on popular authors if possible.
                 
                 If sampling is not possible, the tool may check server roots to filter books by author "Martin".
                 
                 If the list is empty - just display a message that no books are available. Do not return null or throw an error and not call the tool again.
                 """)]
    public static async Task<List<string>> ListRecommendedBookNames(RequestContext<CallToolRequestParams> context)
    {
        if (!_initialized)
        {
            var response = await context.Server.ElicitAsync<YesNoElicitation>("Do you agree to load a list of recommended books?");
            if (response.IsAccepted || response.Action == "accepted")
                _initialized = true;
            else return [];
        }

        var sample = await TrySamplingAsync(context.Server);
        if (!string.IsNullOrWhiteSpace(sample))
            return RecommendedBooks.Where(b => b.Contains(sample)).ToList();

        var roots = await TryGetServerRootsAsync(context.Server);
        if (roots != null)
            return RecommendedBooks.Where(b=>b.Contains("Martin")).ToList();

        return RecommendedBooks;
    }

    private static async Task<string?> TryGetServerRootsAsync(McpServer server)
    {
        try
        {
            var roots = await server.RequestRootsAsync(new ListRootsRequestParams());
            return roots.Roots.Select(r => r.Uri).FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private static async Task<string?> TrySamplingAsync(McpServer server)
    {
        try
        {
            var sample = await server.SampleAsync([new ChatMessage(ChatRole.Assistant, "Analyse these books and find the most popular author name. Respond only author name: " + RecommendedBooks.ToJson())]);

            return sample.Text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}