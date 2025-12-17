using System.ComponentModel;
using ModelContextProtocol.Server;

namespace McpTestServer.Core.Resources;

[McpServerResourceType]
public static class HelloResource
{
    [McpServerResource(MimeType = "text/markdown", UriTemplate = "hello://echo/best", Title = "Get the best greetings message.")]
    [Description("""
                 This resource display the list of best greetings messages in Markdown format.
                 
                 Display the message with valid formatting.
                 """)]
    public static string BestGreetings()
    {
        return """
               ### Warm & elegant

               **Hello!** 🌿
               Wishing you a day filled with peace, gentle moments, and bright surprises. I’m so glad you’re here.

               ### Short & sweet

               **Hi there!** ✨
               May today be kind to you and bring you something to smile about.

               ### Romantic / poetic

               **Hello, lovely soul.** 🌙
               May your heart feel light, your thoughts feel calm, and your day unfold like a beautiful story.

               ### Friendly & cheerful

               **Hey!** 🌸
               Sending you a big, happy hello and a little extra sunshine for your day!

               ### Formal (for work or clients)

               **Good day!**
               Wishing you a wonderful day ahead and a warm welcome. Please let me know how I can help.

               If you tell me *who it’s for* (friend, crush, boss, customer) and the *occasion* (morning, birthday, holiday, general hello), I’ll tailor a perfect one.

               """;
    }
}