using System.ComponentModel;
using System.Net.Http.Json;
using McpTestServer.Core.Extensions;
using McpTestServer.Core.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace McpTestServer.Core.Tools;

public class CurrencyTool(HttpClient client, ILogger<CurrencyTool> logger)
{
    private const string ExchangeApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    private List<NbuCurrency>? _cachedCurrencies;

    [McpServerTool, Description("Provides a list of currency rates for today")]
    public async Task<List<NbuCurrency>> CurrencyRatesAsync()
    {
        return _cachedCurrencies ??= await client.GetFromJsonAsync<List<NbuCurrency>>(ExchangeApiUrl)
            ?? throw new InvalidOperationException("Cannot get currencies from NBU");
    }


    [McpServerTool, Description("Provides the best currency with rate depended on the users' preferences.")]
    public async Task<string> GetBestCurrencyRateAsync(RequestContext<CallToolRequestParams> context)
    {
        var currencyRates = await CurrencyRatesAsync();

        var result = await context.Server.ElicitAsync<CurrencyTypeElicitation>("To what currency do you want to compare with (UAH or USD)");

        logger.LogInformation("The elicitation request has been finished. Response - {Type}", result.ToJson());

        if (!result.IsAccepted || result.Content == null)
            return "The user has cancelled request";

        var selectedCurrency = currencyRates.First(c => c.Cc == result.Content.CurrencyType.ToString());

        return $"The rate for the {result.Content} is {selectedCurrency.ToJson()}";
    }
}