using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using UrlShortener.Core;

namespace UrlShortener.Api;

public sealed class TokenRangeApiClient : ITokenRangeApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =
        HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public TokenRangeApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TokenRangeService");
    }

    public async Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken ct)
    {
        var response = await RetryPolicy.ExecuteAsync(() =>
            _httpClient.PostAsJsonAsync("assign", new { Key = machineKey }, ct)
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to assign new token range");
        }

        return await response.Content.ReadFromJsonAsync<TokenRange>(ct);
    }
}