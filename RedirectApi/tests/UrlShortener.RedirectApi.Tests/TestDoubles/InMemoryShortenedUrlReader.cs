using UrlShortener.RedirectApi.Infrastructure;

namespace UrlShortener.RedirectApi.Tests.TestDoubles;

public class InMemoryShortenedUrlReader : Dictionary<string, ReadLongUrlResponse>, IShortenedUrlReader
{
    public Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        if (TryGetValue(shortUrl, out var response))
        {
            return Task.FromResult(response);
        }

        return Task.FromResult(new ReadLongUrlResponse(false, null));
    }
}