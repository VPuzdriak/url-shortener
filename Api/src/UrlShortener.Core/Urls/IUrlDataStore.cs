namespace UrlShortener.Core.Urls;

public interface IUrlDataStore
{
    Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken);
}