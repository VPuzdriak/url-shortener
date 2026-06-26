namespace UrlShortener.RedirectApi.Infrastructure;

public sealed record ReadLongUrlResponse(bool Found, string? LongUrl);