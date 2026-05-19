namespace UrlShortener.Core.Urls;

public sealed record ShortenedUrl(Uri LongUrl, string ShortUrl, string CreatedBy, DateTimeOffset CreatedOn);