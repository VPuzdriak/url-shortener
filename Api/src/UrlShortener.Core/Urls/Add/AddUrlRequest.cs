namespace UrlShortener.Core.Urls.Add;

public sealed record AddUrlRequest(Uri LongUrl, string CreatedBy);