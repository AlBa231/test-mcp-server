using System.ComponentModel;
using System.Net.Http.Json;
using MCPTestServer.Core.Extensions;
using MCPTestServer.Core.Mcp.Tools.Currency.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace MCPTestServer.Core.Mcp.Tools.Currency;

public class CurrencyTool(HttpClient client, ILogger<CurrencyTool> logger)
{
    private const string ExchangeApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    private List<NbuCurrency>? _cachedCurrencies;

    [McpServerTool(Title = "Provides a list of currency rates for today"), Description("""
                                Use this tool if user asks about currency rates.
                                
                                The tool returns information of currency rates to UAH as base currency.
                                """)]
    public async Task<List<NbuCurrency>> CurrencyRatesAsync()
    {
        return _cachedCurrencies ??= await client.GetFromJsonAsync<List<NbuCurrency>>(ExchangeApiUrl)
            ?? throw new InvalidOperationException("Cannot get currencies from NBU");
    }


    [McpServerTool(Title = "Provides the best currency with rate depended on the users' preferences.")]
    [Description("""
                 Use this tool when user asks to find the best currency rate.
                 
                 The tool will ask the user for additional question using Elicitation to provide the user the currency to convert the rate to (USD or EUR).
                 """)]
    public async Task<string> GetBestCurrencyRateAsync(RequestContext<CallToolRequestParams> context)
    {
        context.Server.AsClientLoggerProvider().CreateLogger("Fetch").Log(LogLevel.Information, "Fetching the currency rates");
        await Task.Delay(3000);

        var currencyRates = await CurrencyRatesAsync();

        var result = await context.Server.ElicitAsync<CurrencyTypeElicitation>("To what currency do you want to compare with (UAH or USD)");

        logger.LogInformation("The elicitation request has been finished. Response - {Type}", result.ToJson());

        if (!result.IsAccepted || result.Content == null)
            return "The user has cancelled request";

        var selectedCurrency = currencyRates.First(c => c.Cc == result.Content.CurrencyType.ToString());

        return $"The rate for the {result.Content} is {selectedCurrency.ToJson()}";
    }
}