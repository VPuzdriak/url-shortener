namespace UrlShortener.RedirectApi.Infrastructure;

public interface IShortenedUrlReader
{
    Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken);
}