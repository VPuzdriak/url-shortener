namespace UrlShortener.Core.Urls.Add;

public sealed record AddUrlResponse(Uri LongUrl, string ShortUrl);